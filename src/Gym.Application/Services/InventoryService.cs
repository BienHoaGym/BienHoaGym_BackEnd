using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Inventory;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

    public InventoryService(IUnitOfWork unitOfWork, IMapper mapper, IAuditLogService auditLogService, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }

    // Warehouse management
    // Internal Supply Management (Direct to warehouse mindset)
    public async Task<ResponseDto<InventoryDto>> CreateInternalSupplyAsync(CreateInternalSupplyDto dto)
    {
        try
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                SKU = "SUP-" + DateTime.Now.Ticks.ToString().Substring(12),
                Type = ProductType.Supply,
                TrackInventory = true,
                CostPrice = dto.CostPrice,
                Price = 0, // Supplies are for internal use, usually no selling price
                Unit = dto.Unit,
                ProviderId = dto.ProviderId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Products.AddAsync(product);
            
            // Add initial stock if provided
            if (dto.InitialQuantity > 0 && dto.WarehouseId.HasValue)
            {
                var inventory = new Inventory
                {
                    ProductId = product.Id,
                    WarehouseId = dto.WarehouseId.Value,
                    Quantity = dto.InitialQuantity,
                    LastUpdated = DateTime.UtcNow
                };
                await _unitOfWork.Inventories.AddAsync(inventory);
                product.StockQuantity = dto.InitialQuantity;

                // Log Transaction
                var transaction = new StockTransaction
                {
                    ProductId = product.Id,
                    ToWarehouseId = dto.WarehouseId.Value,
                    Type = StockTransactionType.Import,
                    Quantity = dto.InitialQuantity,
                    UnitPrice = dto.CostPrice,
                    Date = DateTime.UtcNow,
                    Note = "Khởi tạo vật tư nội bộ trực tiếp",
                    PerformedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống"
                };
                await _unitOfWork.StockTransactions.AddAsync(transaction);
            }

            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<InventoryDto>.SuccessResult(new InventoryDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSKU = product.SKU,
                Quantity = product.StockQuantity,
                CostPrice = product.CostPrice
            }, "Đã thêm vật tư nội bộ trực tiếp vào kho.");
        }
        catch (Exception ex)
        {
            return ResponseDto<InventoryDto>.FailureResult($"Lỗi khi tạo vật tư: {ex.Message}");
        }
    }

    public async Task<ResponseDto<List<WarehouseDto>>> GetWarehousesAsync()
    {
        var warehouses = await _unitOfWork.Warehouses.GetQueryable()
            .Where(w => w.IsActive && !w.IsDeleted)
            .OrderBy(w => w.Name)
            .ToListAsync();
        return ResponseDto<List<WarehouseDto>>.SuccessResult(_mapper.Map<List<WarehouseDto>>(warehouses));
    }

    public async Task<ResponseDto<WarehouseDto>> CreateWarehouseAsync(CreateWarehouseDto dto)
    {
        var warehouse = _mapper.Map<Warehouse>(dto);
        await _unitOfWork.Warehouses.AddAsync(warehouse);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<WarehouseDto>.SuccessResult(_mapper.Map<WarehouseDto>(warehouse), "Tạo kho hàng thành công");
    }

    // Inventory tracking
    public async Task<ResponseDto<List<InventoryDto>>> GetInventoriesAsync(Guid? warehouseId = null, bool includeAssets = false)
    {
        var result = new List<InventoryDto>();

        // 1. Hàng hóa (Products)
        var products = await _unitOfWork.Products.GetQueryable()
            .Where(p => !p.IsDeleted && p.TrackInventory)
            .OrderBy(p => p.Name)
            .ToListAsync();

        foreach (var p in products)
        {
            result.Add(new InventoryDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                ProductSKU = p.SKU,
                Quantity = p.StockQuantity,
                MinStockThreshold = p.MinStockThreshold,
                Price = p.Price,
                CostPrice = p.CostPrice,
                LastUpdated = p.UpdatedAt ?? p.CreatedAt
            });
        }

        // 2. Thiết bị (Assets) - Nếu yêu cầu
        if (includeAssets)
        {
            var equipments = await _unitOfWork.Equipments.GetQueryable()
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.Name)
                .ToListAsync();

            foreach (var e in equipments)
            {
                result.Add(new InventoryDto
                {
                    ProductId = e.Id,
                    ProductName = $"[TÀI SẢN] {e.Name}",
                    ProductSKU = e.EquipmentCode,
                    Quantity = e.Quantity,
                    MinStockThreshold = 0,
                    Price = e.PurchasePrice,
                    CostPrice = e.PurchasePrice,
                    LastUpdated = e.UpdatedAt ?? e.CreatedAt
                });
            }
        }


        return ResponseDto<List<InventoryDto>>.SuccessResult(result.OrderBy(x => x.ProductName).ToList());
    }

    public async Task<ResponseDto<InventoryDto>> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId)
    {
        var inv = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

        if (inv == null) return ResponseDto<InventoryDto>.FailureResult("Không tìm thấy thông tin tồn sản phẩm tại kho này");
        return ResponseDto<InventoryDto>.SuccessResult(_mapper.Map<InventoryDto>(inv));
    }

    // Movement log
    public async Task<ResponseDto<List<StockTransactionDto>>> GetStockTransactionsAsync(Guid? productId = null, Guid? warehouseId = null)
    {
        // 1. Fetch Product Stock Transactions
        var prodQuery = _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .Include(t => t.FromWarehouse)
            .Include(t => t.ToWarehouse)
            .AsNoTracking();

        if (productId.HasValue) prodQuery = prodQuery.Where(t => t.ProductId == productId.Value);
        if (warehouseId.HasValue) 
            prodQuery = prodQuery.Where(t => t.FromWarehouseId == warehouseId.Value || t.ToWarehouseId == warehouseId.Value);

        var prodTransactions = await prodQuery.ToListAsync();
        var result = _mapper.Map<List<StockTransactionDto>>(prodTransactions);

        // 2. Fetch Equipment Transactions (if no warehouse filter or if specifically requested)
        if (!warehouseId.HasValue)
        {
            var equipQuery = _unitOfWork.EquipmentTransactions.GetQueryable()
                .Include(t => t.Equipment)
                .AsNoTracking();

            if (productId.HasValue) equipQuery = equipQuery.Where(t => t.EquipmentId == productId.Value);

            var equipTransactions = await equipQuery.ToListAsync();
            
            foreach(var et in equipTransactions)
            {
                result.Add(new StockTransactionDto
                {
                    Id = et.Id,
                    ProductId = et.EquipmentId,
                    ProductName = $"[TÀI SẢN] {et.Equipment?.Name}",
                    Date = et.Date,
                    Quantity = et.Quantity,
                    Type = ConvertEquipToStockType(et.Type),
                    Note = et.Note,
                    FromWarehouseName = et.FromLocation,
                    ToWarehouseName = et.ToLocation, // Use Location as WarehouseName placeholder
                    PerformedBy = "Hệ thống (Assets)"
                });
            }
        }

        return ResponseDto<List<StockTransactionDto>>.SuccessResult(result.OrderByDescending(t => t.Date).ToList());
    }

    private StockTransactionType ConvertEquipToStockType(EquipmentTransactionType type)
    {
        return type switch
        {
            EquipmentTransactionType.Purchase => StockTransactionType.Import,
            EquipmentTransactionType.Liquidation => StockTransactionType.Export,
            EquipmentTransactionType.Maintenance => StockTransactionType.Adjustment,
            EquipmentTransactionType.Relocation => StockTransactionType.Transfer,
            _ => StockTransactionType.Adjustment
        };
    }

    // Actions
    public async Task<ResponseDto<bool>> ImportStockAsync(CreateStockTransactionDto dto)
    {
        try
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";
            
            if (dto.IsAsset)
            {
                var equipment = await _unitOfWork.Equipments.GetByIdAsync(dto.ProductId);
                if (equipment == null) return ResponseDto<bool>.FailureResult("Không tìm thấy thiết bị để nhập kho");

                int before = equipment.Quantity;
                equipment.Quantity += dto.Quantity;
                equipment.Status = EquipmentStatus.Active;

                var equipTrans = new EquipmentTransaction
                {
                    EquipmentId = equipment.Id,
                    Type = EquipmentTransactionType.Purchase,
                    Quantity = dto.Quantity,
                    BeforeQuantity = before, // Captured earlier
                    AfterQuantity = equipment.Quantity,
                    Date = DateTime.UtcNow,
                    Note = dto.Note ?? $"Nhập kho máy móc/thiết bị qua Quản lý Kho (Số lượng: {dto.Quantity})",
                    ToLocation = equipment.Location,
                    CreatedBy = userName
                };
                await _unitOfWork.EquipmentTransactions.AddAsync(equipTrans);
                _unitOfWork.Equipments.Update(equipment);
                await _unitOfWork.SaveChangesAsync();
                
                return ResponseDto<bool>.SuccessResult(true, $"Đã nhập kho thêm {dto.Quantity} {equipment.Name}.");
            }

            // --- Existing Product logic ---
            if (!dto.ToWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho nhập đến");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

            // 🟢 CHUẨN: Kế thừa + Cập nhật linh hoạt theo phương pháp GIÁ BÌNH QUÂN (Moving Average)
            decimal oldTotalStock = product.StockQuantity;
            decimal oldCostPrice = product.CostPrice;
            decimal incomingQuantity = dto.Quantity;
            decimal incomingPrice = dto.UnitPrice;

            // Inheritance logic: If UnitPrice is not provided (0), inherit from Product's CostPrice
            if (incomingPrice <= 0)
            {
                incomingPrice = oldCostPrice;
                dto.UnitPrice = incomingPrice;
            }

            // Calculate new Average Cost (Giá bình quân gia quyền sau khi nhập)
            // Formula: (SL_cũ * Giá_cũ + SL_mới * Giá_mới) / (SL_cũ + SL_mới)
            decimal totalNewStock = oldTotalStock + incomingQuantity;
            if (totalNewStock > 0)
            {
                decimal totalValueBefore = oldTotalStock * oldCostPrice;
                decimal incomingValue = incomingQuantity * incomingPrice;
                product.CostPrice = (totalValueBefore + incomingValue) / totalNewStock;
            }
            else
            {
                product.CostPrice = incomingPrice;
            }

            var inventory = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);
            int beforeProd = inventory.Quantity;
            inventory.Quantity += dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId(); 
            transaction.Type = StockTransactionType.Import;
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = beforeProd;
            transaction.AfterQuantity = inventory.Quantity;
            transaction.PerformedBy = userName;
            transaction.UnitPrice = incomingPrice; // Store the actual price used in this transaction
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);
            _unitOfWork.Products.Update(product);

            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<bool>.SuccessResult(true, $"Đã nhập kho {dto.Quantity} sản phẩm. Giá nhập: {incomingPrice:N0}đ. Giá bình quân mới: {product.CostPrice:N0}đ");
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult($"Lỗi khi nhập kho: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> ExportStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.FromWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho xuất");

        try
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

            var inventory = await _unitOfWork.Inventories.GetQueryable()
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

            if (inventory == null || inventory.Quantity < dto.Quantity)
                return ResponseDto<bool>.FailureResult("Số lượng tồn kho tại kho này không đủ");

            // Price inheritance for Export
            if (dto.UnitPrice <= 0) dto.UnitPrice = product.CostPrice;

            int before = inventory.Quantity;
            inventory.Quantity -= dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId();
            transaction.Type = StockTransactionType.Export;
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = before;
            transaction.AfterQuantity = inventory.Quantity;
            transaction.PerformedBy = userName;
            transaction.UnitPrice = dto.UnitPrice;
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<bool>.SuccessResult(true, $"Đã xuất kho {dto.Quantity} sản phẩm.");
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult($"Lỗi khi xuất kho: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> TransferStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.FromWarehouseId.HasValue || !dto.ToWarehouseId.HasValue)
            return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho gửi và kho nhận");

        try
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";

            if (dto.IsAsset)
            {
                var equipment = await _unitOfWork.Equipments.GetByIdAsync(dto.ProductId);
                if (equipment == null) return ResponseDto<bool>.FailureResult("Không tìm thấy thiết bị");

                var fromWh = await _unitOfWork.Warehouses.GetByIdAsync(dto.FromWarehouseId.Value);
                var toWh = await _unitOfWork.Warehouses.GetByIdAsync(dto.ToWarehouseId.Value);

                equipment.Location = toWh?.Name ?? "Kho nhận";
                _unitOfWork.Equipments.Update(equipment);

                var equipTx = new EquipmentTransaction
                {
                    EquipmentId = equipment.Id,
                    Type = EquipmentTransactionType.Transfer,
                    Quantity = dto.Quantity,
                    Date = DateTime.UtcNow,
                    FromLocation = fromWh?.Name ?? "Kho gửi",
                    ToLocation = toWh?.Name ?? "Kho nhận",
                    Note = $"Điều chuyển nội bộ: {dto.Note}"
                };
                await _unitOfWork.EquipmentTransactions.AddAsync(equipTx);
            }
            else
            {
                var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
                if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

                var sourceInv = await _unitOfWork.Inventories.GetQueryable()
                    .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

                // TRƯỜNG HỢP: Kho đi chưa có bản ghi tồn kho cụ thể
                if (sourceInv == null)
                {
                    // Nếu sản phẩm đã có số lượng tồn tổng (Global) nhưng chưa chia về kho này
                    if (product.StockQuantity >= dto.Quantity)
                    {
                        // Giả định hàng đang ở kho đi này và tự động khởi tạo bản ghi cho kho đó
                        sourceInv = new Inventory
                        {
                            ProductId = product.Id,
                            WarehouseId = dto.FromWarehouseId.Value,
                            Quantity = product.StockQuantity,
                            LastUpdated = DateTime.UtcNow
                        };
                        await _unitOfWork.Inventories.AddAsync(sourceInv);
                    }
                    else
                    {
                        return ResponseDto<bool>.FailureResult("Kho này chưa có hàng và tổng tồn kho cũng không đủ");
                    }
                }

                if (sourceInv.Quantity < dto.Quantity)
                    return ResponseDto<bool>.FailureResult("Tồn kho nguồn không đủ để điều chuyển");

                var destInv = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);

                int sourceBefore = sourceInv.Quantity;
                sourceInv.Quantity -= dto.Quantity;
                destInv.Quantity += dto.Quantity;

                sourceInv.LastUpdated = DateTime.UtcNow;
                destInv.LastUpdated = DateTime.UtcNow;

                _unitOfWork.Inventories.Update(sourceInv);
                _unitOfWork.Inventories.Update(destInv);

                var transaction = _mapper.Map<StockTransaction>(dto);
                transaction.CheckId();
                transaction.Type = StockTransactionType.Transfer;
                transaction.Date = DateTime.UtcNow;
                transaction.BeforeQuantity = sourceBefore;
                transaction.AfterQuantity = sourceInv.Quantity;
                transaction.PerformedBy = userName;
                await _unitOfWork.StockTransactions.AddAsync(transaction);
                
                // Đồng bộ hóa lại số lượng tổng cuối cùng
                await UpdateProductGlobalStock(dto.ProductId);
            }

            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<bool>.SuccessResult(true, "Điều chuyển kho thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult($"Lỗi điều chuyển: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> InternalUseStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.FromWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho xuất dùng");

        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

            var inventory = await _unitOfWork.Inventories.GetQueryable()
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

            if (inventory == null || inventory.Quantity < dto.Quantity)
                return ResponseDto<bool>.FailureResult("Tồn kho không đủ để xuất dùng");

            // Price inheritance for Internal Use
            if (dto.UnitPrice <= 0) dto.UnitPrice = product.CostPrice;

            inventory.Quantity -= dto.Quantity;
            // _unitOfWork.Inventories.Update(inventory);
            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId();
            transaction.Type = StockTransactionType.InternalUse;
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = inventory.Quantity + dto.Quantity;
            transaction.AfterQuantity = inventory.Quantity;
            transaction.PerformedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";
            transaction.UnitPrice = dto.UnitPrice;
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<bool>.SuccessResult(true, "Đã xuất dùng nội bộ thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult($"Lỗi xuất dùng: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> StockAdjustmentAsync(CreateStockTransactionDto dto)
    {
        if (!dto.ToWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Chọn kho muốn điều chỉnh");

        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

            string userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";
            string message = "Điều chỉnh tồn kho thành công";
            int before = 0;
            int after = (int)dto.Quantity;

            // TRƯỜNG HỢP: Có cả kho đi và kho đến -> Thực hiện Luân chuyển (Subtract source, Add dest)
            if (dto.FromWarehouseId.HasValue && dto.FromWarehouseId != dto.ToWarehouseId)
            {
                var sourceInv = await _unitOfWork.Inventories.GetQueryable()
                    .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

                if (sourceInv == null || sourceInv.Quantity < dto.Quantity)
                    return ResponseDto<bool>.FailureResult("Tồn kho nguồn không đủ để điều phối");

                var destInv = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);

                before = sourceInv.Quantity;
                sourceInv.Quantity -= (int)dto.Quantity;
                destInv.Quantity += (int)dto.Quantity;

                // EF Core tracking handles these updates automatically
                
                dto.Type = StockTransactionType.Transfer;
                after = sourceInv.Quantity; 
                message = "Đã điều phối số lượng giữa 2 kho thành công";
            }
            // TRƯỜNG HỢP: Chỉ có 1 kho -> Thiết lập số lượng chính xác (Set Exact)
            else
            {
                var inventory = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);
                before = inventory.Quantity;
                inventory.Quantity = (int)dto.Quantity; 
                
                // EF Core tracking handles this
                dto.Type = StockTransactionType.Adjustment;
            }

            // Price inheritance for Adjustment
            if (dto.UnitPrice <= 0) dto.UnitPrice = product.CostPrice;

            // No need to explicitly call _unitOfWork.Inventories.Update(inventory) or (sourceInv/destInv)
            // as EF Core tracks changes to entities retrieved or added to the context.

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId();
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = before;
            transaction.AfterQuantity = after;
            transaction.PerformedBy = userName;
            transaction.UnitPrice = dto.UnitPrice;
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            // SAVE FIRST to ensure all Inventory records exist in DB for the sum calculation
            await _unitOfWork.SaveChangesAsync();

            // Calculate global stock AFTER save
            await UpdateProductGlobalStock(dto.ProductId);
            await _unitOfWork.SaveChangesAsync();

            // ✅ GHI AUDIT LOG: Đây là hành động Critical (Sửa kho thủ công)
            await _auditLogService.LogAsync("System", "MANUAL_STOCK_ADJUSTMENT", "Inventories", 
                null, new { productId = dto.ProductId, warehouseId = dto.ToWarehouseId, quantity = dto.Quantity });

            return ResponseDto<bool>.SuccessResult(true, message);
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult($"Lỗi điều chỉnh: {ex.Message}");
        }
    }

    public async Task<ResponseDto<Order>> CreateOrderAsync(Order order, Guid warehouseId)
    {
        try
        {
            order.CreatedDate = DateTime.UtcNow;
            var count = await _unitOfWork.Orders.GetQueryable().CountAsync();
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{(count + 1):D4}";

            decimal total = 0;
            foreach (var detail in order.OrderDetails)
            {
                var inventory = await _unitOfWork.Inventories.GetQueryable()
                    .FirstOrDefaultAsync(i => i.ProductId == detail.ProductId && i.WarehouseId == warehouseId);
                if (inventory == null || inventory.Quantity < detail.Quantity)
                    return ResponseDto<Order>.FailureResult($"Sản phẩm {detail.ProductId} không đủ tồn tại kho bán hàng");

                inventory.Quantity -= detail.Quantity;
                 // EF Core tracking handles this
                
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    await UpdateProductGlobalStock(product.Id);
                    detail.Price = product.Price;
                }
                
                total += detail.Price * detail.Quantity;

                // Log Export
                var transaction = new StockTransaction
                {
                    ProductId = detail.ProductId,
                    FromWarehouseId = warehouseId,
                    Type = StockTransactionType.Export,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.Price,
                    Date = DateTime.UtcNow,
                    Note = $"Bán hàng tại quầy: {order.OrderNumber}",
                    ReferenceNumber = order.OrderNumber,
                    BeforeQuantity = inventory.Quantity + detail.Quantity,
                    AfterQuantity = inventory.Quantity,
                    PerformedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống"
                };
                transaction.CheckId();
                await _unitOfWork.StockTransactions.AddAsync(transaction);
            }

            order.TotalAmount = total;
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return ResponseDto<Order>.SuccessResult(order, "Tạo đơn hàng thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<Order>.FailureResult($"Lỗi tạo đơn: {ex.Message}");
        }
    }

    public async Task<ResponseDto<List<InventoryDto>>> GetStockAlertsAsync()
    {
        // 1. Get products where sum of stock across warehouses <= threshold
        var productAlerts = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .Where(i => !i.Product.IsDeleted && i.Product.IsActive && !i.Warehouse.IsDeleted && i.Quantity <= i.Product.MinStockThreshold)
            .ToListAsync();
        
        var result = _mapper.Map<List<InventoryDto>>(productAlerts);

        // 2. Get equipments that need attention (Status is Broken or NeedsMaintenance)
        var equipAlerts = await _unitOfWork.Equipments.GetQueryable()
            .Where(e => !e.IsDeleted && (e.Status == EquipmentStatus.Maintenance || e.Status == EquipmentStatus.Broken))
            .ToListAsync();

        foreach(var e in equipAlerts)
        {
            result.Add(new InventoryDto
            {
                ProductId = e.Id,
                ProductName = $"[TÀI SẢN] {e.Name} (CẦN SỬA CHỮA)",
                ProductSKU = e.EquipmentCode,
                WarehouseName = e.Location ?? "Phòng tập",
                Quantity = e.Quantity,
                MinStockThreshold = 1, // Placeholder
                LastUpdated = e.UpdatedAt ?? e.CreatedAt
            });
        }
            
        return ResponseDto<List<InventoryDto>>.SuccessResult(result);
    }

    // Helpers
    private async Task<Inventory> GetOrCreateInventory(Guid productId, Guid warehouseId)
    {
        // Phải reload từ DB hoặc lấy từ Context để đảm bảo Id duy nhất
        var inventory = await _unitOfWork.Inventories.GetQueryable()
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

        if (inventory == null)
        {
            inventory = new Inventory 
            { 
               Id = Guid.NewGuid(), // Gán chủ động để tránh duplicate Guid.Empty
               ProductId = productId, 
               WarehouseId = warehouseId, 
               Quantity = 0, 
               LastUpdated = DateTime.UtcNow 
            };
            await _unitOfWork.Inventories.AddAsync(inventory);
        }
        return inventory;
    }

    private async Task UpdateProductGlobalStock(Guid productId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product != null)
        {
            // Tính toán lại tổng tồn từ Context (Local tracking sẽ được ToListAsync hợp nhất)
            var inventories = await _unitOfWork.Inventories.GetQueryable()
                .Where(i => i.ProductId == productId)
                .ToListAsync();
            
            var totalStockRecalculated = inventories.Sum(i => i.Quantity);
            
            if (product.StockQuantity != totalStockRecalculated)
            {
                product.StockQuantity = totalStockRecalculated;
                _unitOfWork.Products.Update(product);
            }
        }
    }
}

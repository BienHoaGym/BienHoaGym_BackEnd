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
    public async Task<ResponseDto<InventoryDto>> CreateInternalSupplyAsync(CreateInternalSupplyDto dto)
    {
        try
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                SKU = "SUP-" + DateTime.UtcNow.Ticks.ToString().Substring(12),
                Type = ProductType.Supply,
                TrackInventory = true,
                CostPrice = dto.CostPrice,
                Price = 0, 
                Unit = dto.Unit,
                ProviderId = dto.ProviderId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Products.AddAsync(product);
            
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

        if (!warehouseId.HasValue || productId.HasValue)
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
                    ToWarehouseName = et.ToLocation, 
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
                equipment.Quantity += (int)dto.Quantity;
                equipment.Status = EquipmentStatus.Active;

                var equipTrans = new EquipmentTransaction
                {
                    EquipmentId = equipment.Id,
                    Type = EquipmentTransactionType.Purchase,
                    Quantity = (int)dto.Quantity,
                    BeforeQuantity = before,
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

            if (!dto.ToWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho nhập đến");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

            decimal oldTotalStock = product.StockQuantity;
            decimal oldCostPrice = product.CostPrice;
            decimal incomingQuantity = dto.Quantity;
            decimal incomingPrice = dto.UnitPrice;

            if (incomingPrice <= 0)
            {
                incomingPrice = oldCostPrice;
                dto.UnitPrice = incomingPrice;
            }

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
            inventory.Quantity += (int)dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId(); 
            transaction.Type = StockTransactionType.Import;
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = beforeProd;
            transaction.AfterQuantity = inventory.Quantity;
            transaction.PerformedBy = userName;
            transaction.UnitPrice = incomingPrice;
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);
            _unitOfWork.Products.Update(product);

            await _unitOfWork.SaveChangesAsync();
            await UpdateProductGlobalStock(dto.ProductId);
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

            if (dto.UnitPrice <= 0) dto.UnitPrice = product.CostPrice;

            int before = inventory.Quantity;
            inventory.Quantity -= (int)dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

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
            await UpdateProductGlobalStock(dto.ProductId);
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
                    Quantity = (int)dto.Quantity,
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

                if (sourceInv == null || sourceInv.Quantity < dto.Quantity)
                    return ResponseDto<bool>.FailureResult("Tồn kho nguồn không đủ để điều chuyển");

                var destInv = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);

                int sourceBefore = sourceInv.Quantity;
                sourceInv.Quantity -= (int)dto.Quantity;
                destInv.Quantity += (int)dto.Quantity;

                sourceInv.LastUpdated = DateTime.UtcNow;
                destInv.LastUpdated = DateTime.UtcNow;

                var transaction = _mapper.Map<StockTransaction>(dto);
                transaction.CheckId();
                transaction.Type = StockTransactionType.Transfer;
                transaction.Date = DateTime.UtcNow;
                transaction.BeforeQuantity = sourceBefore;
                transaction.AfterQuantity = sourceInv.Quantity;
                transaction.PerformedBy = userName;
                await _unitOfWork.StockTransactions.AddAsync(transaction);
                
                await _unitOfWork.SaveChangesAsync();
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

            if (dto.UnitPrice <= 0) dto.UnitPrice = product.CostPrice;

            int before = inventory.Quantity;
            inventory.Quantity -= (int)dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId();
            transaction.Type = StockTransactionType.InternalUse;
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = before;
            transaction.AfterQuantity = inventory.Quantity;
            transaction.PerformedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";
            transaction.UnitPrice = dto.UnitPrice;
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await UpdateProductGlobalStock(dto.ProductId);
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
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

            string userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống";
            string message = "Điều chỉnh tồn kho thành công";
            int before = 0;
            int after = (int)dto.Quantity;

            if (dto.FromWarehouseId.HasValue && dto.ToWarehouseId.HasValue && dto.FromWarehouseId != dto.ToWarehouseId)
            {
                var sourceInv = await _unitOfWork.Inventories.GetQueryable()
                    .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

                if (sourceInv == null || sourceInv.Quantity < dto.Quantity)
                    return ResponseDto<bool>.FailureResult("Tồn kho nguồn không đủ để điều phối");

                var destInv = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);

                before = sourceInv.Quantity;
                sourceInv.Quantity -= (int)dto.Quantity;
                destInv.Quantity += (int)dto.Quantity;
                
                dto.Type = StockTransactionType.Transfer;
                after = sourceInv.Quantity; 
                message = "Đã điều phối số lượng giữa 2 kho thành công";
            }
            else if (dto.ToWarehouseId.HasValue)
            {
                var inventory = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);
                before = inventory.Quantity;
                inventory.Quantity = (int)dto.Quantity; 
                dto.Type = StockTransactionType.Adjustment;
            }
            else
            {
                return ResponseDto<bool>.FailureResult("Bắt buộc phải chọn kho để điều chỉnh.");
            }

            if (dto.UnitPrice <= 0) dto.UnitPrice = product.CostPrice;

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.CheckId();
            transaction.Date = DateTime.UtcNow;
            transaction.BeforeQuantity = before;
            transaction.AfterQuantity = after;
            transaction.PerformedBy = userName;
            transaction.UnitPrice = dto.UnitPrice;
            
            await _unitOfWork.StockTransactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            await UpdateProductGlobalStock(dto.ProductId);
            await _unitOfWork.SaveChangesAsync();

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
            order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";

            decimal total = 0;
            foreach (var detail in order.OrderDetails)
            {
                var inventory = await _unitOfWork.Inventories.GetQueryable()
                    .FirstOrDefaultAsync(i => i.ProductId == detail.ProductId && i.WarehouseId == warehouseId);
                if (inventory == null || inventory.Quantity < detail.Quantity)
                    return ResponseDto<Order>.FailureResult($"Sản phẩm {detail.ProductId} không đủ tồn tại kho bán hàng");

                inventory.Quantity -= (int)detail.Quantity;
                
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    detail.Price = product.Price;
                }
                
                total += detail.Price * detail.Quantity;

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
                    BeforeQuantity = inventory.Quantity + (int)detail.Quantity,
                    AfterQuantity = inventory.Quantity,
                    PerformedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Hệ thống"
                };
                transaction.CheckId();
                await _unitOfWork.StockTransactions.AddAsync(transaction);
            }

            order.TotalAmount = total;
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Recalculate global stock for all updated products
            foreach (var detail in order.OrderDetails)
            {
                await UpdateProductGlobalStock(detail.ProductId);
            }
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
        var productAlerts = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .Where(i => !i.Product.IsDeleted && i.Product.IsActive && !i.Warehouse.IsDeleted && i.Quantity <= i.Product.MinStockThreshold)
            .ToListAsync();
        
        var result = _mapper.Map<List<InventoryDto>>(productAlerts);

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
                MinStockThreshold = 1, 
                LastUpdated = e.UpdatedAt ?? e.CreatedAt
            });
        }
            
        return ResponseDto<List<InventoryDto>>.SuccessResult(result);
    }

    // Helpers
    private async Task<Inventory> GetOrCreateInventory(Guid productId, Guid warehouseId)
    {
        var inventory = await _unitOfWork.Inventories.GetQueryable()
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

        if (inventory == null)
        {
            inventory = new Inventory 
            { 
               Id = Guid.NewGuid(), 
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

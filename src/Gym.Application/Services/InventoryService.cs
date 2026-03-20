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

    public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Warehouse management
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
    public async Task<ResponseDto<List<InventoryDto>>> GetInventoriesAsync(Guid? warehouseId = null)
    {
        var query = _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .AsNoTracking();

        if (warehouseId.HasValue)
            query = query.Where(i => i.WarehouseId == warehouseId.Value);

        var inventories = await query.OrderBy(i => i.Product.Name).ToListAsync();
        return ResponseDto<List<InventoryDto>>.SuccessResult(_mapper.Map<List<InventoryDto>>(inventories));
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
        var query = _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .Include(t => t.FromWarehouse)
            .Include(t => t.ToWarehouse)
            .AsNoTracking();

        if (productId.HasValue) query = query.Where(t => t.ProductId == productId.Value);
        if (warehouseId.HasValue) 
            query = query.Where(t => t.FromWarehouseId == warehouseId.Value || t.ToWarehouseId == warehouseId.Value);

        var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();
        return ResponseDto<List<StockTransactionDto>>.SuccessResult(_mapper.Map<List<StockTransactionDto>>(transactions));
    }

    // Actions
    public async Task<ResponseDto<bool>> ImportStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.ToWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho nhập đến");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventory = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);
            inventory.Quantity += dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            _unitOfWork.Inventories.Update(inventory);

            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.Type = StockTransactionType.Import;
            transaction.Date = DateTime.UtcNow;
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return ResponseDto<bool>.SuccessResult(true, $"Đã nhập kho {dto.Quantity} sản phẩm.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<bool>.FailureResult($"Lỗi khi nhập kho: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> ExportStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.FromWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho xuất");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventory = await _unitOfWork.Inventories.GetQueryable()
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

            if (inventory == null || inventory.Quantity < dto.Quantity)
                return ResponseDto<bool>.FailureResult("Số lượng tồn kho tại kho này không đủ");

            inventory.Quantity -= dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            _unitOfWork.Inventories.Update(inventory);

            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.Type = StockTransactionType.Export;
            transaction.Date = DateTime.UtcNow;
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return ResponseDto<bool>.SuccessResult(true, $"Đã xuất kho {dto.Quantity} sản phẩm.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<bool>.FailureResult($"Lỗi khi xuất kho: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> TransferStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.FromWarehouseId.HasValue || !dto.ToWarehouseId.HasValue)
            return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho gửi và kho nhận");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Decrease from source
            var sourceInv = await _unitOfWork.Inventories.GetQueryable()
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

            if (sourceInv == null || sourceInv.Quantity < dto.Quantity)
                return ResponseDto<bool>.FailureResult("Số lượng tại kho gửi không đủ");

            sourceInv.Quantity -= dto.Quantity;
            _unitOfWork.Inventories.Update(sourceInv);

            // Increase at destination
            var destInv = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);
            destInv.Quantity += dto.Quantity;
            _unitOfWork.Inventories.Update(destInv);

            // Log transaction
            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.Type = StockTransactionType.Transfer;
            transaction.Date = DateTime.UtcNow;
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return ResponseDto<bool>.SuccessResult(true, "Điều chuyển kho thành công");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<bool>.FailureResult($"Lỗi điều chuyển: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> InternalUseStockAsync(CreateStockTransactionDto dto)
    {
        if (!dto.FromWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Bắt buộc chọn kho xuất dùng");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventory = await _unitOfWork.Inventories.GetQueryable()
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId.Value);

            if (inventory == null || inventory.Quantity < dto.Quantity)
                return ResponseDto<bool>.FailureResult("Tồn kho không đủ để xuất dùng");

            inventory.Quantity -= dto.Quantity;
            _unitOfWork.Inventories.Update(inventory);
            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.Type = StockTransactionType.InternalUse;
            transaction.Date = DateTime.UtcNow;
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return ResponseDto<bool>.SuccessResult(true, "Đã xuất dùng nội bộ thành công");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<bool>.FailureResult($"Lỗi xuất dùng: {ex.Message}");
        }
    }

    public async Task<ResponseDto<bool>> StockAdjustmentAsync(CreateStockTransactionDto dto)
    {
        if (!dto.ToWarehouseId.HasValue) return ResponseDto<bool>.FailureResult("Chọn kho muốn điều chỉnh");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventory = await GetOrCreateInventory(dto.ProductId, dto.ToWarehouseId.Value);
            inventory.Quantity = dto.Quantity; // Set exact
            inventory.LastUpdated = DateTime.UtcNow;
            _unitOfWork.Inventories.Update(inventory);

            await UpdateProductGlobalStock(dto.ProductId);

            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.Type = StockTransactionType.Adjustment;
            transaction.Date = DateTime.UtcNow;
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return ResponseDto<bool>.SuccessResult(true, "Điều chỉnh tồn kho thành công");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<bool>.FailureResult($"Lỗi điều chỉnh: {ex.Message}");
        }
    }

    public async Task<ResponseDto<Order>> CreateOrderAsync(Order order, Guid warehouseId)
    {
        await _unitOfWork.BeginTransactionAsync();
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
                {
                    await _unitOfWork.RollbackAsync();
                    return ResponseDto<Order>.FailureResult($"Sản phẩm không đủ tồn tại kho bán hàng");
                }

                inventory.Quantity -= detail.Quantity;
                _unitOfWork.Inventories.Update(inventory);
                
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    await UpdateProductGlobalStock(product.Id);
                    detail.Price = product.Price;
                }
                
                total += detail.Price * detail.Quantity;

                // Log Export
                await _unitOfWork.StockTransactions.AddAsync(new StockTransaction
                {
                    ProductId = detail.ProductId,
                    FromWarehouseId = warehouseId,
                    Type = StockTransactionType.Export,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.Price,
                    Date = DateTime.UtcNow,
                    Note = $"Bán hàng tại quầy: {order.OrderNumber}",
                    ReferenceNumber = order.OrderNumber
                });
            }

            order.TotalAmount = total;
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return ResponseDto<Order>.SuccessResult(order, "Tạo đơn hàng thành công");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<Order>.FailureResult($"Lỗi tạo đơn: {ex.Message}");
        }
    }

    public async Task<ResponseDto<List<InventoryDto>>> GetStockAlertsAsync()
    {
        // Get products where sum of stock across warehouses <= threshold
        var alerts = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .Where(i => i.Quantity <= i.Product.MinStockThreshold)
            .ToListAsync();
            
        return ResponseDto<List<InventoryDto>>.SuccessResult(_mapper.Map<List<InventoryDto>>(alerts));
    }

    // Helpers
    private async Task<Inventory> GetOrCreateInventory(Guid productId, Guid warehouseId)
    {
        var inventory = await _unitOfWork.Inventories.GetQueryable()
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

        if (inventory == null)
        {
            inventory = new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = 0, LastUpdated = DateTime.UtcNow };
            await _unitOfWork.Inventories.AddAsync(inventory);
        }
        return inventory;
    }

    private async Task UpdateProductGlobalStock(Guid productId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product != null)
        {
            var totalStock = await _unitOfWork.Inventories.GetQueryable()
                .Where(i => i.ProductId == productId)
                .SumAsync(i => i.Quantity);
            
            product.StockQuantity = totalStock;
            _unitOfWork.Products.Update(product);
        }
    }
}

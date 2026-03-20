using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Inventory;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ResponseDto<List<InventoryDto>>> GetInventoriesAsync()
    {
        var inventories = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .OrderBy(i => i.Product.Name)
            .ToListAsync();
        
        return ResponseDto<List<InventoryDto>>.SuccessResult(_mapper.Map<List<InventoryDto>>(inventories));
    }

    public async Task<ResponseDto<InventoryDto>> GetByProductIdAsync(Guid productId)
    {
        var inv = await _unitOfWork.Inventories.FindAsync(i => i.ProductId == productId);
        var res = inv.FirstOrDefault();
        if (res == null) return ResponseDto<InventoryDto>.FailureResult("Inventory record not found");
        return ResponseDto<InventoryDto>.SuccessResult(_mapper.Map<InventoryDto>(res));
    }

    public async Task<ResponseDto<List<StockTransactionDto>>> GetStockTransactionsAsync(Guid? productId = null)
    {
        var query = _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .AsQueryable();
        
        if (productId.HasValue) query = query.Where(t => t.ProductId == productId.Value);
        
        var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();
        return ResponseDto<List<StockTransactionDto>>.SuccessResult(_mapper.Map<List<StockTransactionDto>>(transactions));
    }

    public async Task<ResponseDto<bool>> ImportStockAsync(CreateStockTransactionDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Update or Create Inventory
            var inventory = (await _unitOfWork.Inventories.FindAsync(i => i.ProductId == dto.ProductId)).FirstOrDefault();
            if (inventory == null)
            {
                inventory = new Inventory { ProductId = dto.ProductId, Quantity = dto.Quantity, LastUpdated = DateTime.UtcNow };
                await _unitOfWork.Inventories.AddAsync(inventory);
            }
            else
            {
                inventory.Quantity += dto.Quantity;
                inventory.LastUpdated = DateTime.UtcNow;
                _unitOfWork.Inventories.Update(inventory);
            }

            // Also update Product.StockQuantity for backward compatibility
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product != null)
            {
                product.StockQuantity = inventory.Quantity;
                _unitOfWork.Products.Update(product);
            }

            // Record Transaction
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
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventory = (await _unitOfWork.Inventories.FindAsync(i => i.ProductId == dto.ProductId)).FirstOrDefault();
            if (inventory == null || inventory.Quantity < dto.Quantity)
                return ResponseDto<bool>.FailureResult("Số lượng tồn kho không đủ.");

            inventory.Quantity -= dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            _unitOfWork.Inventories.Update(inventory);

            // Sync Product
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product != null)
            {
                product.StockQuantity = inventory.Quantity;
                _unitOfWork.Products.Update(product);
            }

            // Record Transaction
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

    public async Task<ResponseDto<bool>> StockAdjustmentAsync(CreateStockTransactionDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventory = (await _unitOfWork.Inventories.FindAsync(i => i.ProductId == dto.ProductId)).FirstOrDefault();
            if (inventory == null)
            {
                inventory = new Inventory { ProductId = dto.ProductId, Quantity = dto.Quantity, LastUpdated = DateTime.UtcNow };
                await _unitOfWork.Inventories.AddAsync(inventory);
            }
            else
            {
                inventory.Quantity = dto.Quantity; // Set to exact value for adjustment
                inventory.LastUpdated = DateTime.UtcNow;
                _unitOfWork.Inventories.Update(inventory);
            }

            // Sync Product
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product != null)
            {
                product.StockQuantity = inventory.Quantity;
                _unitOfWork.Products.Update(product);
            }

            // Record Transaction
            var transaction = _mapper.Map<StockTransaction>(dto);
            transaction.Type = StockTransactionType.Adjustment;
            transaction.Date = DateTime.UtcNow;
            await _unitOfWork.StockTransactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return ResponseDto<bool>.SuccessResult(true, $"Đã điều chỉnh kho thành {dto.Quantity}.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<bool>.FailureResult($"Lỗi khi điều chỉnh kho: {ex.Message}");
        }
    }

    public async Task<ResponseDto<Order>> CreateOrderAsync(Order order)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            order.CreatedDate = DateTime.UtcNow;
            
            // Generate order number
            var count = await _unitOfWork.Orders.GetQueryable().CountAsync();
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{(count + 1):D4}";

            decimal total = 0;
            foreach (var detail in order.OrderDetails)
            {
                // Verify Product and Stock
                var inventory = (await _unitOfWork.Inventories.FindAsync(i => i.ProductId == detail.ProductId)).FirstOrDefault();
                if (inventory == null || inventory.Quantity < detail.Quantity)
                {
                    await _unitOfWork.RollbackAsync();
                    return ResponseDto<Order>.FailureResult($"Sản phẩm {detail.ProductId} không đủ tồn kho.");
                }

                inventory.Quantity -= detail.Quantity;
                inventory.LastUpdated = DateTime.UtcNow;
                _unitOfWork.Inventories.Update(inventory);
                
                // Sync Product
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    product.StockQuantity = inventory.Quantity;
                    _unitOfWork.Products.Update(product);
                    detail.Price = product.Price; // Use current price if not set
                }
                
                total += detail.Price * detail.Quantity;

                // Record Stock Transaction (Export)
                var stockTx = new StockTransaction
                {
                    ProductId = detail.ProductId,
                    Type = StockTransactionType.Export,
                    Quantity = detail.Quantity,
                    Date = DateTime.UtcNow,
                    Note = $"Bán hàng đơn {order.OrderNumber}",
                    ReferenceNumber = order.OrderNumber
                };
                await _unitOfWork.StockTransactions.AddAsync(stockTx);
            }

            order.TotalAmount = total;
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return ResponseDto<Order>.SuccessResult(order, "Đã tạo đơn hàng thành công.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResponseDto<Order>.FailureResult($"Lỗi khi tạo đơn hàng: {ex.Message}");
        }
    }
    public async Task<ResponseDto<List<InventoryDto>>> GetStockAlertsAsync()
    {
        var alerts = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .Where(i => i.Quantity <= i.Product.MinStockThreshold)
            .ToListAsync();
            
        return ResponseDto<List<InventoryDto>>.SuccessResult(_mapper.Map<List<InventoryDto>>(alerts));
    }
}

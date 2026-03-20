using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Inventory;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Services;

public interface IInventoryService
{
    // Inventory tracking
    Task<ResponseDto<List<InventoryDto>>> GetInventoriesAsync();
    Task<ResponseDto<InventoryDto>> GetByProductIdAsync(Guid productId);
    
    // Movement log
    Task<ResponseDto<List<StockTransactionDto>>> GetStockTransactionsAsync(Guid? productId = null);
    
    // Actions
    Task<ResponseDto<bool>> ImportStockAsync(CreateStockTransactionDto dto);
    Task<ResponseDto<bool>> ExportStockAsync(CreateStockTransactionDto dto);
    Task<ResponseDto<bool>> StockAdjustmentAsync(CreateStockTransactionDto dto);

    // Orders (Sản phẩm)
    Task<ResponseDto<Order>> CreateOrderAsync(Order order);

    // Alerts
    Task<ResponseDto<List<InventoryDto>>> GetStockAlertsAsync();
}

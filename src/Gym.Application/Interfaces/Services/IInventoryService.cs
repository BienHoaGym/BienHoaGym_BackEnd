using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Inventory;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Services;

public interface IInventoryService
{
    // Warehouse management
    Task<ResponseDto<List<WarehouseDto>>> GetWarehousesAsync();
    Task<ResponseDto<WarehouseDto>> CreateWarehouseAsync(CreateWarehouseDto dto);
    Task<ResponseDto<InventoryDto>> CreateInternalSupplyAsync(CreateInternalSupplyDto dto);
    
    // Inventory tracking
    Task<ResponseDto<List<InventoryDto>>> GetInventoriesAsync(Guid? warehouseId = null, bool includeAssets = false);
    Task<ResponseDto<InventoryDto>> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId);
    
    // Movement log
    Task<ResponseDto<List<StockTransactionDto>>> GetStockTransactionsAsync(Guid? productId = null, Guid? warehouseId = null);
    
    // Actions
    Task<ResponseDto<bool>> ImportStockAsync(CreateStockTransactionDto dto);
    Task<ResponseDto<bool>> ExportStockAsync(CreateStockTransactionDto dto);
    Task<ResponseDto<bool>> TransferStockAsync(CreateStockTransactionDto dto); // Chuyển kho
    Task<ResponseDto<bool>> InternalUseStockAsync(CreateStockTransactionDto dto); // Xuất dùng nội bộ
    Task<ResponseDto<bool>> StockAdjustmentAsync(CreateStockTransactionDto dto); // Điều chỉnh tồn kho


    // Orders
    Task<ResponseDto<Order>> CreateOrderAsync(Order order, Guid warehouseId);

    // Alerts
    Task<ResponseDto<List<InventoryDto>>> GetStockAlertsAsync();
}

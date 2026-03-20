using Gym.Domain.Enums;

namespace Gym.Application.DTOs.Inventory;

public class InventoryDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class StockTransactionDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public StockTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderName { get; set; }
}

public class CreateStockTransactionDto
{
    public Guid ProductId { get; set; }
    public StockTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public Guid? ProviderId { get; set; }
}

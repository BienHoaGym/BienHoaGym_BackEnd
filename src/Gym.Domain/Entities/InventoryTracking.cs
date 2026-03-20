using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Inventory tracking for a product (1:1 with Product)
/// </summary>
public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Historical record of stock changes (Import / Export)
/// </summary>
public class StockTransaction : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public StockTransactionType Type { get; set; } // Import, Export
    public int Quantity { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    public string? Note { get; set; }
    public string? ReferenceNumber { get; set; } // PO-X, SO-X
    
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
}

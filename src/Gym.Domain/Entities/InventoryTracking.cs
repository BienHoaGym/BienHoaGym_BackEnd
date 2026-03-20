using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Warehouse entity - Kho hàng (Kho tổng, Kho quầy, Kho vệ sinh...)
/// </summary>
public class Warehouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}

/// <summary>
/// Inventory tracking for a product (1:1 with Product per Warehouse)
/// </summary>
public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;

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

    public Guid? FromWarehouseId { get; set; }
    public virtual Warehouse? FromWarehouse { get; set; }

    public Guid? ToWarehouseId { get; set; }
    public virtual Warehouse? ToWarehouse { get; set; }

    public StockTransactionType Type { get; set; } // Import, Export, Transfer...
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Giá nhập/xuất tại thời điểm giao dịch
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    public string? Note { get; set; }
    public string? ReferenceNumber { get; set; } // PO-X, SO-X
    
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
}

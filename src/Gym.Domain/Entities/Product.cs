using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Product entity - Sản phẩm (Nước uống, thực phẩm chức năng, phụ kiện...)
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty; // Mã sản phẩm
    public decimal Price { get; set; }
    public string Unit { get; set; } = "Cái"; // Chai, Gói, kg...
    public ProductType Type { get; set; } = ProductType.Retail;
    public int StockQuantity { get; set; }
    public int MinStockThreshold { get; set; } = 5; 
    public int MaxStockThreshold { get; set; } = 100;
    public DateTime? ExpirationDate { get; set; } 
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
    public bool IsActive { get; set; } = true;
}

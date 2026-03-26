using Gym.Domain.Common;
using Gym.Domain.Enums;
using System;

namespace Gym.Domain.Entities;

/// <summary>
/// Product entity - Quản lý tất cả sản phẩm, dịch vụ, vật tư trong phòng gym
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty; // Mã sản phẩm
    public string? Barcode { get; set; } // Mã vạch
    
    public decimal Price { get; set; } // Giá bán
    public decimal CostPrice { get; set; } // Giá vốn (để tính lợi nhuận)
    
    public string Unit { get; set; } = "Cái"; // Chai, Gói, kg, Buổi, Tháng...
    public ProductType Type { get; set; } = ProductType.Retail;
    
    // Inventory related
    public int StockQuantity { get; set; } // Tổng tồn kho across warehouses (for sync)
    public int MinStockThreshold { get; set; } = 5; 
    public int MaxStockThreshold { get; set; } = 100;
    public DateTime? ExpirationDate { get; set; } 
    public bool TrackInventory { get; set; } = true; // True cho hàng hóa, False cho dịch vụ
    
    // Service related
    public int? DurationDays { get; set; } // Số ngày hiệu lực (nếu là dịch vụ)
    public int? SessionCount { get; set; } // Số buổi (nếu là gói tập/PT)
    
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
    
    public bool IsActive { get; set; } = true;
}

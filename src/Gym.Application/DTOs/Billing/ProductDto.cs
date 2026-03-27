using System;

namespace Gym.Application.DTOs.Billing;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    
    public string Unit { get; set; } = string.Empty;
    public int Type { get; set; } // 1: Service, 2: Supply, 3: Retail
    
    public int StockQuantity { get; set; }
    public int MinStockThreshold { get; set; }
    public int MaxStockThreshold { get; set; }
    public bool TrackInventory { get; set; }
    
    public int? DurationDays { get; set; }
    public int? SessionCount { get; set; }
    
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderName { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    
    public string Unit { get; set; } = "Cái";
    public int Type { get; set; } = 1;
    
    public bool TrackInventory { get; set; } = true;
    public int? MinStockThreshold { get; set; } = 5;
    public int? MaxStockThreshold { get; set; } = 100;
    
    public int? DurationDays { get; set; }
    public int? SessionCount { get; set; }
    
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ProviderId { get; set; }
}

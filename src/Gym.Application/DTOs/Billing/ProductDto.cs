namespace Gym.Application.DTOs.Billing;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int Type { get; set; } // 1: Retail, 2: Supplement, 3: Supply
    public int StockQuantity { get; set; }
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
    public decimal Price { get; set; }
    public string Unit { get; set; } = "Cái";
    public int Type { get; set; } = 1;
    public int StockQuantity { get; set; }
    public int MinStockThreshold { get; set; } = 5;
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ProviderId { get; set; }
}

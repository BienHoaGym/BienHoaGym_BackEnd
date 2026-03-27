using System;

namespace Gym.Application.DTOs.Inventory;

public class CreateInternalSupplyDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal CostPrice { get; set; }
    public string Unit { get; set; } = "Cái";
    public Guid? ProviderId { get; set; }
    
    // Initial Stock details
    public Guid? WarehouseId { get; set; }
    public int InitialQuantity { get; set; }
}

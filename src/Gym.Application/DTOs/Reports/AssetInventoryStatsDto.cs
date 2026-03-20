namespace Gym.Application.DTOs.Reports;

public class AssetInventoryStatsDto
{
    // Inventory
    public int TotalProducts { get; set; }
    public int TotalStockItems { get; set; }
    public decimal TotalStockValue { get; set; }
    public List<TopProductItemDto> LowStockItems { get; set; } = new();

    // Equipment
    public int TotalEquipments { get; set; }
    public decimal TotalOriginalValue { get; set; }
    public decimal TotalMaintenanceCosts { get; set; }
    public decimal TotalCurrentValue { get; set; } // After depreciation
    public List<EquipmentStatusCountDto> StatusCounts { get; set; } = new();
}

public class TopProductItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class EquipmentStatusCountDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

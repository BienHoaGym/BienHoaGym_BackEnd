namespace Gym.Application.DTOs.Equipment;

public class EquipmentCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
    public decimal? AvgMaintenanceCost { get; set; }
    public int? StandardWarrantyMonths { get; set; }
    public int EquipmentCount { get; set; }
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
    public decimal? AvgMaintenanceCost { get; set; }
    public int? StandardWarrantyMonths { get; set; }
}

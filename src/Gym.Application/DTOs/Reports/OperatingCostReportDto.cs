using System.Collections.Generic;

namespace Gym.Application.DTOs.Reports;

public class OperatingCostReportDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    
    // Part 1: CHI PHÍ VẬT TƯ (Consumables)
    public decimal TotalMaterialCost { get; set; }
    public List<MaterialCostDetailDto> MaterialDetails { get; set; } = new();

    // Part 2: CHI PHÍ KHẤU HAO (Depreciation)
    public decimal TotalDepreciationCost { get; set; }
    public List<DepreciationCostDetailDto> DepreciationDetails { get; set; } = new();

    // Summary
    public decimal TotalOperatingCost => TotalMaterialCost + TotalDepreciationCost;
}

public class MaterialCostDetailDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int Quantity { get; set; }
}

public class DepreciationCostDetailDto
{
    public string EquipmentName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Count { get; set; } // Số lượng máy cùng loại được khấu hao
}

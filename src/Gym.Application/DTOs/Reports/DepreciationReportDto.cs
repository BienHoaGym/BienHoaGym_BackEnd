using System;
using System.Collections.Generic;

namespace Gym.Application.DTOs.Reports;

public class DepreciationReportDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalDepreciationAmount { get; set; }
    public int EquipmentCount { get; set; }
    
    public List<DepreciationByCategoryDto> ByCategory { get; set; } = new();
    public List<EquipmentDepreciationDetailDto> Details { get; set; } = new();
}

public class DepreciationByCategoryDto
{
    public string CategoryName { get; set; } = string.Empty;
    public int EquipmentCount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class EquipmentDepreciationDetailDto
{
    public Guid EquipmentId { get; set; }
    public string EquipmentCode { get; set; } = string.Empty;
    public string EquipmentName { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public decimal DepreciationAmount { get; set; }
    public decimal RemainingValue { get; set; }
    public string Status { get; set; } = string.Empty;
}

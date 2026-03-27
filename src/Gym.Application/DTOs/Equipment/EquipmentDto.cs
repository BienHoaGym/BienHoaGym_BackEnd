using Gym.Domain.Enums;

namespace Gym.Application.DTOs.Equipment;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public string EquipmentCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderName { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? WarrantyExpiryDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public EquipmentStatus Status { get; set; }
    public EquipmentPriority Priority { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public double? Weight { get; set; }
    public int MaintenanceIntervalDays { get; set; }
    public int UsefulLifeMonths { get; set; }
    public decimal SalvageValue { get; set; }
    public DateTime? DepreciationStartDate { get; set; }
    public decimal MonthlyDepreciationAmount { get; set; }
    public decimal AccumulatedDepreciation { get; set; }
    public decimal CurrentBookValue { get; set; }
    public bool IsFullyDepreciated { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
}

public class CreateEquipmentDto
{
    public string? EquipmentCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ProviderId { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? WarrantyExpiryDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Active;
    public EquipmentPriority Priority { get; set; } = EquipmentPriority.Medium;
    public string? Location { get; set; }
    public string? Description { get; set; }
    public double? Weight { get; set; }
    public int MaintenanceIntervalDays { get; set; } = 90;
    public int UsefulLifeMonths { get; set; } = 36;
    public decimal SalvageValue { get; set; } = 0;
    public decimal MonthlyDepreciationAmount { get; set; } = 0;
    public DateTime? DepreciationStartDate { get; set; }
}

public class EquipmentTransactionDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public EquipmentTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
}

public class CreateEquipmentTransactionDto
{
    public Guid EquipmentId { get; set; }
    public EquipmentTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
}

public class MaintenanceLogDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Technician { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderName { get; set; }
    public MaintenanceStatus Status { get; set; }
    public List<MaintenanceMaterialDto> Materials { get; set; } = new();
}

public class MaintenanceMaterialDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateMaintenanceLogDto
{
    public Guid EquipmentId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledDate { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Technician { get; set; }
    public Guid? ProviderId { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Completed;
    public List<CreateMaintenanceMaterialDto> UsedMaterials { get; set; } = new();
}

public class CreateMaintenanceMaterialDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid WarehouseId { get; set; } // Kho để xuất vật tư (thường là Kho Quầy hoặc Kho Tổng)
}

public class DepreciationDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public decimal RemainingValue { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }
}

public class IncidentLogDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string? ReportedBy { get; set; }
    public string ResolutionStatus { get; set; } = string.Empty;
}

public class CreateIncidentLogDto
{
    public Guid EquipmentId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium";
    public string? ReportedBy { get; set; }
    public string ResolutionStatus { get; set; } = "Open";
}

public class EquipmentProviderHistoryDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid? OldProviderId { get; set; }
    public string? OldProviderName { get; set; }
    public Guid? NewProviderId { get; set; }
    public string? NewProviderName { get; set; }
    public DateTime ChangeDate { get; set; }
    public string? Reason { get; set; }
}

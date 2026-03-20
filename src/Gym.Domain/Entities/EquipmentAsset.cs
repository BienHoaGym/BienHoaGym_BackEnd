using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Equipments (Assets) - Các máy móc, trang thiết bị trong gym (Tạ, giàn tập, thảm...)
/// </summary>
public class Equipment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string EquipmentCode { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public Guid? CategoryId { get; set; }
    public virtual EquipmentCategory? EquipmentCategory { get; set; }

    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }

    public int Quantity { get; set; } // Tổng số lượng
    public DateTime PurchaseDate { get; set; }
    public DateTime? WarrantyExpiryDate { get; set; } // Ngày hết hạn bảo hành
    public decimal PurchasePrice { get; set; }
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Active;
    public EquipmentPriority Priority { get; set; } = EquipmentPriority.Medium;
    public string? Location { get; set; } // Vị trí lắp đặt/ Khu vực
    public string? Description { get; set; }

    // Maintenance scheduling
    public int MaintenanceIntervalDays { get; set; } = 90; // Mặc định 3 tháng bảo trì 1 lần
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }

    public virtual ICollection<EquipmentTransaction> Transactions { get; set; } = new List<EquipmentTransaction>();
    public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
    public virtual ICollection<Depreciation> Depreciations { get; set; } = new List<Depreciation>();
    public virtual ICollection<IncidentLog> IncidentLogs { get; set; } = new List<IncidentLog>();
    public virtual ICollection<EquipmentProviderHistory> ProviderHistories { get; set; } = new List<EquipmentProviderHistory>();
}

/// <summary>
/// Ghi nhận lịch sử giao dịch thiết bị: Mua mới, Điều chuyển, Thanh lý
/// </summary>
public class EquipmentTransaction : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; } = null!;

    public EquipmentTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
}

/// <summary>
/// Nhật ký bảo trì, sửa chữa thiết bị
/// </summary>
public class MaintenanceLog : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledDate { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Technician { get; set; }
    
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }

    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Completed;
}

/// <summary>
/// Khấu hao tài sản (Accounting)
/// </summary>
public class Depreciation : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; } = null!;

    public decimal Value { get; set; } // Giá trị khấu hao ghi nhận
    public DateTime Date { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Nhật ký sự cố thiết bị
/// </summary>
public class IncidentLog : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium"; // Low, Medium, High, Critical
    public string? ReportedBy { get; set; }
    public string ResolutionStatus { get; set; } = "Open"; // Open, InProgress, Resolved
}

/// <summary>
/// Lịch sử thay đổi nhà cung cấp của thiết bị
/// </summary>
public class EquipmentProviderHistory : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; } = null!;

    public Guid? OldProviderId { get; set; }
    public virtual Provider? OldProvider { get; set; }

    public Guid? NewProviderId { get; set; }
    public virtual Provider? NewProvider { get; set; }

    public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
}

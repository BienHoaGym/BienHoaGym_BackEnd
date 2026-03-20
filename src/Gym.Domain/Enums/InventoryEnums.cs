namespace Gym.Domain.Enums;

public enum StockTransactionType
{
    Import = 1, // Nhập kho
    Export = 2, // Xuất kho
    Adjustment = 3 // Điều chuyển / Kiểm kho
}

public enum EquipmentStatus
{
    Active = 1,      // Đang hoạt động
    Broken = 2,      // Đang hỏng
    Maintenance = 3, // Đang bảo trình
    Liquidated = 4   // Đã thanh lý
}

public enum EquipmentTransactionType
{
    Purchase = 1,  // Mua mới
    Transfer = 2,  // Điều chuyển
    Liquidation = 3    // Thanh lý
}

public enum MaintenanceStatus
{
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}

public enum EquipmentPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

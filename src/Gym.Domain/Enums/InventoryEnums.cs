namespace Gym.Domain.Enums;

public enum StockTransactionType
{
    Import = 1,           // Nhập kho (từ NCC)
    Export = 2,           // Xuất kho (bán hàng)
    Adjustment = 3,       // Điều chỉnh kho (kiểm kê)
    Transfer = 4,         // Điều chuyển nội bộ (giữa các kho)
    Damage = 5,           // Hàng hỏng
    Loss = 6,             // Hao hụt / Mất mát
    InternalUse = 7,      // Xuất dùng nội bộ (cho PT, lễ tân, vệ sinh)
    Return = 8            // Xuất trả hàng NCC
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
    Liquidation = 3,   // Thanh lý
    Maintenance = 4,   // Bảo trì
    Relocation = 5     // Di chuyển vị trí
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

public enum ProductType
{
    Service = 1,      // Gói tập / Dịch vụ PT (Không tồn kho)
    Supply = 2,       // Vật tư vận hành (Khăn, xà bông...) - Quản lý tồn kho
    Retail = 3        // Hàng bán lẻ (Nước, Whey...) - Quản lý tồn kho đơn giản
}

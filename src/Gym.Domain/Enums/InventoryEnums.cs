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

public enum ProductType
{
    Retail = 1,       // Hàng bán lẻ (nước, đồ tập, phụ kiện)
    Supplement = 2,   // Thực phẩm bổ sung (Whey, Gain...)
    Supply = 3        // Vật tư vận hành (khăn, nước rửa tay, giấy...)
}

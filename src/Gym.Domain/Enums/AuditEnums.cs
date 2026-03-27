namespace Gym.Domain.Enums;

/// <summary>
/// Mức độ quan trọng của sự kiện Audit Log
/// </summary>
public enum AuditSeverity
{
    /// <summary>
    /// Bình thường (Ghi 20%) - Ví dụ: Check-in thông thường
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Quan trọng (Ghi 80%) - Ví dụ: Gia hạn gói tập, điền danh lớp đông...
    /// </summary>
    Important = 1,

    /// <summary>
    /// Nghiêm trọng (Ghi 100%) - Ví dụ: Đổi giá, xóa hóa đơn, đổi quyền Admin...
    /// </summary>
    Critical = 2
}

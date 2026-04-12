using System;

namespace Gym.Domain.Enums;

/// <summary>
/// Trạng thái phân công PT cho hội viên
/// </summary>
public enum TrainerAssignmentStatus
{
    /// <summary>
    /// Chờ phân công PT (Vừa mua gói nhưng chưa chọn PT)
    /// </summary>
    PendingAssignment = 0,

    /// <summary>
    /// Đang tập luyện (Đã gán PT và đang hoạt động)
    /// </summary>
    Active = 1,

    /// <summary>
    /// Đã hoàn thành (Kết thúc số buổi tập)
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Đã hủy (Chấm dứt hợp đồng sớm)
    /// </summary>
    Cancelled = 3
}

using Gym.Domain.Common;

namespace Gym.Domain.Entities;

/// <summary>
/// Class entity - Lớp tập (Yoga, Boxing...)
/// </summary>
public class Class : BaseEntity
{
    public string ClassName { get; set; } = string.Empty;

    public string? ClassType { get; set; } // Yoga, Zumba, Boxing, Dance, HIIT...

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    /// <summary>
    /// Ngày trong tuần (Monday, Tuesday...)
    /// </summary>
    public string? ScheduleDay { get; set; }

    /// <summary>
    /// Giờ bắt đầu
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Giờ kết thúc
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Sức chứa tối đa
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Số member đã đăng ký
    /// </summary>
    public int CurrentEnrollment { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Trainer Trainer { get; set; } = null!;

    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; set; } = new List<ClassEnrollment>();
}

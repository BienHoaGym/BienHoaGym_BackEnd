using Gym.Domain.Common;

namespace Gym.Domain.Entities;

/// <summary>
/// TrainerMemberAssignment entity - Liên kết giữa Huấn luyện viên và Hội viên
/// </summary>
public class TrainerMemberAssignment : BaseEntity
{
    public Guid TrainerId { get; set; }
    public virtual Trainer Trainer { get; set; } = null!;

    public Guid MemberId { get; set; }
    public virtual Member Member { get; set; } = null!;

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
}

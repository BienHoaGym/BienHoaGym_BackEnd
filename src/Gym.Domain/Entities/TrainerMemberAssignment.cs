using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// TrainerMemberAssignment entity - Hợp đồng huấn luyện (Liên kết giữa Huấn luyện viên và Hội viên)
/// </summary>
public class TrainerMemberAssignment : BaseEntity
{
    public Guid? TrainerId { get; set; }
    public virtual Trainer? Trainer { get; set; }

    public Guid MemberId { get; set; }
    public virtual Member Member { get; set; } = null!;

    public Guid? MemberSubscriptionId { get; set; }
    public virtual MemberSubscription? MemberSubscription { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    public TrainerAssignmentStatus Status { get; set; } = TrainerAssignmentStatus.PendingAssignment;

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
}

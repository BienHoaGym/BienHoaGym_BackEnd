using Gym.Domain.Common;

namespace Gym.Domain.Entities;

public class CheckIn : BaseEntity
{
    public Guid MemberId { get; set; }
    public virtual Member? Member { get; set; } // Dùng d? l?y FullName

    public Guid? SubscriptionId { get; set; }
    public virtual MemberSubscription? Subscription { get; set; } // Dùng d? l?y tên Package

    public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
    public DateTime? CheckOutTime { get; set; }
    public string? Note { get; set; }
}

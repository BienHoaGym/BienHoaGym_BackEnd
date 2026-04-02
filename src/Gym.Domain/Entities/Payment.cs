using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

public class Payment : BaseEntity
{
    // 1. S?a l?i 'SubscriptionId': Ð?t tên th?ng nh?t là MemberSubscriptionId
    public Guid MemberSubscriptionId { get; set; }
    public virtual MemberSubscription? Subscription { get; set; }

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // 2. S?a l?i 'PaymentMethod': Ð?t tên thu?c tính là Method (Ki?u Enum PaymentMethod)
    // Luu ý: Trong Service ph?i g?i là .Method, không ph?i .PaymentMethod
    public PaymentMethod Method { get; set; } = PaymentMethod.Cash;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? TransactionId { get; set; }
    public string? Note { get; set; }

    // Ph?i là tên này:

    // Ph?i là tên này:
}

using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

public class Payment : BaseEntity
{
    // 1. Sửa lỗi 'SubscriptionId': Đặt tên thống nhất là MemberSubscriptionId
    public Guid MemberSubscriptionId { get; set; }
    public virtual MemberSubscription? Subscription { get; set; }

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // 2. Sửa lỗi 'PaymentMethod': Đặt tên thuộc tính là Method (Kiểu Enum PaymentMethod)
    // Lưu ý: Trong Service phải gọi là .Method, không phải .PaymentMethod
    public PaymentMethod Method { get; set; } = PaymentMethod.Cash;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? TransactionId { get; set; }
    public string? Note { get; set; }

    // Phải là tên này:

    // Phải là tên này:
}
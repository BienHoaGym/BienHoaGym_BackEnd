using Gym.Domain.Common;

namespace Gym.Domain.Entities;

public class MembershipPackage : BaseEntity
{
    // Đổi 'PackageName' thành 'Name' (chuẩn Clean Code)
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }

    // Sửa 'DurationDays' thành 'DurationInDays' để rõ nghĩa hơn
    public int DurationInDays { get; set; }
    public int DurationInMonths { get; set; } // Giữ lại nếu cần tính theo tháng

    public int? SessionLimit { get; set; }
    public bool HasPT { get; set; } = false; // "Có PT hay không có PT"
    public bool IsActive { get; set; } = true;

    // Navigation Property
    public virtual ICollection<MemberSubscription> Subscriptions { get; set; } = new List<MemberSubscription>();
}
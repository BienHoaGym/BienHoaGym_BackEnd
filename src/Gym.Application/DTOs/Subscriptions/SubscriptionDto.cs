namespace Gym.Application.DTOs.Subscriptions;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Guid PackageId { get; set; }

    public string MemberName { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;

    // --- THÊM MỚI: Giá gói tập ---
    public decimal PackagePrice { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DaysRemaining => (int)(EndDate - DateTime.Today).TotalDays;
    public string Status { get; set; } = string.Empty;

    //public int Id { get; set; }
    //public string PackageName { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    //public string Status { get; set; }
    //public DateTime StartDate { get; set; }
    //public DateTime EndDate { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}



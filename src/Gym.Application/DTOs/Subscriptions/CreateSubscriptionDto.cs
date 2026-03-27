namespace Gym.Application.DTOs.Subscriptions;


public class CreateSubscriptionDto
{
    public Guid MemberId { get; set; } // Đảm bảo là Guid 
    public Guid PackageId { get; set; } // Đảm bảo là Guid [cite: 160]
    public DateTime StartDate { get; set; }
    public string? UserId { get; set; } = null;
    public string? VoucherCode { get; set; } = null;
    public string? PaymentMethod { get; set; } = null;
    public decimal? FinalPrice { get; set; }
}


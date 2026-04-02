namespace Gym.Application.DTOs.Payments;

public class ProcessPaymentDto
{
    public Guid MemberSubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}

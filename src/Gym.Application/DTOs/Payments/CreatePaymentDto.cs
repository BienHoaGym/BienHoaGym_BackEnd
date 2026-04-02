using System.ComponentModel.DataAnnotations;

namespace Gym.Application.DTOs.Payments;

public class CreatePaymentDto
{
    [Required]
    public Guid MemberSubscriptionId { get; set; } // Tr? ti?n cho dang ký nào

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "S? ti?n ph?i l?n hon 0")]
    public decimal Amount { get; set; }

    [Required]
    public string PaymentMethod { get; set; } = "Cash"; // M?c d?nh là ti?n m?t

    public string? TransactionId { get; set; } // Tùy ch?n
}

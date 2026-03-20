using System.ComponentModel.DataAnnotations;

namespace Gym.Application.DTOs.Payments;

public class CreatePaymentDto
{
    [Required]
    public Guid MemberSubscriptionId { get; set; } // Trả tiền cho đăng ký nào

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
    public decimal Amount { get; set; }

    [Required]
    public string PaymentMethod { get; set; } = "Cash"; // Mặc định là tiền mặt

    public string? TransactionId { get; set; } // Tùy chọn
}
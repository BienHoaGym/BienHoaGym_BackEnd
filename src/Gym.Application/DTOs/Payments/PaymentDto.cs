namespace Gym.Application.DTOs.Payments;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid MemberSubscriptionId { get; set; }
    public string MemberName { get; set; } = string.Empty; // Tên hội viên
    public string PackageName { get; set; } = string.Empty; // Tên gói
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // Cash, Transfer, Card
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; } // Mã giao dịch ngân hàng (nếu có)
    public string Status { get; set; } = string.Empty;
}
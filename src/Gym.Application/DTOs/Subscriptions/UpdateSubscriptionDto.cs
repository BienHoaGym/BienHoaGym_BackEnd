namespace Gym.Application.DTOs.Subscriptions;

public class UpdateSubscriptionDto
{
    public Guid Id { get; set; }
    public DateTime? EndDate { get; set; } // Gia hạn thủ công
    public string Status { get; set; } = string.Empty; // Cập nhật trạng thái (Active/Expired/Cancelled)
}
namespace Gym.Application.DTOs.Subscriptions;

public class UpdateSubscriptionDto
{
    public Guid Id { get; set; }
    public DateTime? EndDate { get; set; } // Gia h?n th? công
    public string Status { get; set; } = string.Empty; // C?p nh?t tr?ng thái (Active/Expired/Cancelled)
}

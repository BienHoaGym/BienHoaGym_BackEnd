namespace Gym.Application.DTOs.Subscriptions;

public class SubscriptionListDto
{
    public Guid Id { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

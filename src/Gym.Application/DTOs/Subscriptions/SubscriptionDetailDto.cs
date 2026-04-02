using Gym.Application.DTOs.Members;
using Gym.Application.DTOs.Packages;
using Gym.Application.DTOs.Payments;

namespace Gym.Application.DTOs.Subscriptions;

public class SubscriptionDetailDto
{
    public Guid Id { get; set; }

    public MemberDto Member { get; set; } = null!;

    public PackageDto Package { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? TotalSessions { get; set; }

    public int? RemainingSession { get; set; }

    public bool IsAutoRenew { get; set; }

    public List<PaymentDto> Payments { get; set; } = new();

    public int DaysRemaining => (EndDate - DateTime.Today).Days;

    public bool IsExpiringSoon => DaysRemaining <= 7 && DaysRemaining > 0;
}

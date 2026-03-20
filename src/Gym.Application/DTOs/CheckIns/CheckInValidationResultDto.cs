using Gym.Application.DTOs.Members;
using Gym.Application.DTOs.Subscriptions;

namespace Gym.Application.DTOs.CheckIns;

public class CheckInValidationResultDto
{
    public bool IsValid { get; set; }

    public string Message { get; set; } = string.Empty;

    public MemberDto? Member { get; set; }

    public SubscriptionDto? ActiveSubscription { get; set; }

    public List<string> Errors { get; set; } = new();

    public List<string> Warnings { get; set; } = new();
}
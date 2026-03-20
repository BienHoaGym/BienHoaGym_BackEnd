namespace Gym.Application.DTOs.Members;

/// <summary>
/// DTO for member list view (simplified)
/// </summary>
public class MemberListDto
{
    public Guid Id { get; set; }

    public string MemberCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Active subscription end date (if any)
    /// </summary>
    public DateTime? ActiveSubscriptionEndDate { get; set; }
}
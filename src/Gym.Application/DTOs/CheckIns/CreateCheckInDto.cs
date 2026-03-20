namespace Gym.Application.DTOs.CheckIns;

public class CreateCheckInDto
{
    public string MemberCode { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
namespace Gym.Application.DTOs.CheckIns;

public class CheckInHistoryDto
{
    public Guid Id { get; set; }
    public DateTime CheckInTime { get; set; }
    public string Status { get; set; } = string.Empty;
}
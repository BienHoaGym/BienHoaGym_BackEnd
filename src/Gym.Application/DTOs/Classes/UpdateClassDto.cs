namespace Gym.Application.DTOs.Classes;

public class UpdateClassDto
{
    public string ClassName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? TrainerId { get; set; }

    public string? ScheduleDay { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int MaxCapacity { get; set; }

    public bool IsActive { get; set; }
}

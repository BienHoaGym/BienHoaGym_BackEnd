using Gym.Application.DTOs.Trainers;

namespace Gym.Application.DTOs.Classes;

public class ClassDto
{
    public Guid Id { get; set; }

    public string ClassName { get; set; } = string.Empty;

    public string? ClassType { get; set; }

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    public string TrainerName { get; set; } = string.Empty;

    public string? ScheduleDay { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int MaxCapacity { get; set; }

    public int CurrentEnrollment { get; set; }

    public int AvailableSlots => MaxCapacity - CurrentEnrollment;

    public bool IsActive { get; set; }

    public bool IsFull => CurrentEnrollment >= MaxCapacity;
}

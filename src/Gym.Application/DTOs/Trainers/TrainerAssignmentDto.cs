namespace Gym.Application.DTOs.Trainers;

public class TrainerAssignmentDto
{
    public Guid Id { get; set; }
    public Guid TrainerId { get; set; }
    public string TrainerName { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTrainerAssignmentDto
{
    public Guid TrainerId { get; set; }
    public Guid MemberId { get; set; }
    public string? Notes { get; set; }
}

namespace Gym.Application.DTOs.Trainers;

public class CreateTrainerDto
{
    public Guid? UserId { get; set; }
    public string? TrainerCode { get; set; }
    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Specialization { get; set; }

    public string? Bio { get; set; }

    public DateTime? HireDate { get; set; }
    public decimal Salary { get; set; }
    public decimal SessionRate { get; set; }
}
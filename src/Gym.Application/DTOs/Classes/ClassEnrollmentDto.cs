namespace Gym.Application.DTOs.Classes;

public class ClassEnrollmentDto
{
    public Guid ClassId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime EnrolledDate { get; set; }
}
namespace Gym.Application.DTOs.Classes;

public class ClassAttendanceDto
{
    public Guid EnrollmentId { get; set; }
    public Guid ClassId { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public DateTime AttendanceDate { get; set; }
    public bool IsAttended { get; set; }
}

public class MarkAttendanceDto
{
    public Guid EnrollmentId { get; set; }
    public bool IsPresent { get; set; }
}

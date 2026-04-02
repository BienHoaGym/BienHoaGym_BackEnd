using Gym.Domain.Common;

namespace Gym.Domain.Entities;

public class ClassEnrollment : BaseEntity
{
    public Guid ClassId { get; set; }
    public virtual Class? Class { get; set; }

    public Guid MemberId { get; set; }
    public virtual Member? Member { get; set; }

    public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;

    public bool IsAttended { get; set; } = false;
    public DateTime? AttendanceDate { get; set; }
}

using Gym.Domain.Common;
using System.Security.Claims;

namespace Gym.Domain.Entities;

/// <summary>
/// Trainer entity - Huấn luyện viên
/// </summary>
public class Trainer : BaseEntity
{
    public Guid? UserId { get; set; }

    public string TrainerCode { get; set; } = string.Empty; // Mã PT

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Chuyên môn (Yoga, Boxing, Personal Training...)
    /// </summary>
    public string? Specialization { get; set; }

    /// <summary>
    /// Số năm kinh nghiệm huấn luyện
    /// </summary>
    public int ExperienceYears { get; set; }

    /// <summary>
    /// Tiểu sử, kinh nghiệm chi tiết
    /// </summary>
    public string? Bio { get; set; }

    public string? ProfilePhoto { get; set; }

    public DateTime? HireDate { get; set; }

    public decimal Salary { get; set; }
    public decimal SessionRate { get; set; } // Giá mỗi buổi PT

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual User? User { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    public virtual ICollection<TrainerMemberAssignment> TrainerMemberAssignments { get; set; } = new List<TrainerMemberAssignment>();
}

using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

public class Member : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string MemberCode { get; set; } = string.Empty;
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

    public MemberStatus Status { get; set; } = MemberStatus.Active;

    // --- CÁC TRƯỜNG BỔ SUNG (Cần thêm vào DB) ---
    public string? Gender { get; set; }        // Nam/Nữ
    public string? Address { get; set; }       // Địa chỉ
    public string? Note { get; set; }          // Ghi chú
    public string? EmergencyContact { get; set; } // Liên hệ khẩn cấp (Tên)
    public string? EmergencyPhone { get; set; }   // Liên hệ khẩn cấp (SĐT)

    public string? FaceEncoding { get; set; }   // Vector đặc trưng khuôn mặt (JSON string/Base64)
    public string? QRCode { get; set; }         // Dữ liệu mã QR duy nhất để check-in
    // ---------------------------------------------

    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual ICollection<MemberSubscription> Subscriptions { get; set; } = new List<MemberSubscription>();
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; set; } = new List<ClassEnrollment>();
    public virtual ICollection<TrainerMemberAssignment> TrainerMemberAssignments { get; set; } = new List<TrainerMemberAssignment>();
}
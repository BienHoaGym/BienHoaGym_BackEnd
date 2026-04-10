namespace Gym.Application.DTOs.Members;

public class UpdateMemberDto
{
    public Guid Id { get; set; } // Thêm ID để kiểm tra an toàn
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public string? Notes { get; set; } // Giữ tên này để khớp với Form, Mapping sẽ xử lý sang entity.Note

    public string? FaceEncoding { get; set; }
}

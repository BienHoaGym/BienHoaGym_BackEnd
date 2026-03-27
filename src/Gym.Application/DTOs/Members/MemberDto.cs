namespace Gym.Application.DTOs.Members;

public class MemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinedDate { get; set; } // Khớp với Entity
    public string Status { get; set; } = string.Empty;

    // Bổ sung các trường để Frontend không bị "N/A"
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }

    public string? FaceEncoding { get; set; }
}
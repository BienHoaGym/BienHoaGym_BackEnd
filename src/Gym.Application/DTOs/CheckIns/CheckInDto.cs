namespace Gym.Application.DTOs.CheckIns;

public class CheckInDto
{
    public Guid Id { get; set; }
    public string MemberName { get; set; } = string.Empty; // Map t? Member.FullName
    public string MemberCode { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty; // Thêm d? hi?n tên gói t?p
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string? Notes { get; set; }
}

// DTO m?i d? fix l?i truy?n string t? Body
public class ValidateCheckInRequest
{
    public string MemberCode { get; set; } = string.Empty;
}

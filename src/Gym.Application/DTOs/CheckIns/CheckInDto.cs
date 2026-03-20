namespace Gym.Application.DTOs.CheckIns;

public class CheckInDto
{
    public Guid Id { get; set; }
    public string MemberName { get; set; } = string.Empty; // Map từ Member.FullName
    public string MemberCode { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty; // Thêm để hiện tên gói tập
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string? Notes { get; set; }
}

// DTO mới để fix lỗi truyền string từ Body
public class ValidateCheckInRequest
{
    public string MemberCode { get; set; } = string.Empty;
}
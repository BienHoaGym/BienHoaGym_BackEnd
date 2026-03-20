namespace Gym.Application.DTOs.Members;

public class RegisterLeadDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PackageInterest { get; set; }
    public string? Notes { get; set; }
}

namespace Gym.Application.DTOs.Users;

public class UserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty; // Primary role
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();

    public bool IsActive { get; set; }

    public DateTime? LastLoginAt { get; set; }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gym.Domain.Entities;

/// <summary>
/// User entity - Tài khoản đăng nhập hệ thống
/// </summary>
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? IdentityNumber { get; set; } // CCCD
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; } // Nam, Nữ, Khác
    public DateTime? HireDate { get; set; }
    public string? BankCardNumber { get; set; }
    public string? BankName { get; set; }
    
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual Member? Member { get; set; }

    public virtual Trainer? Trainer { get; set; }
}

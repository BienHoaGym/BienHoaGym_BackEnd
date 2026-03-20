using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.Domain.Entities;

/// <summary>
/// Role entity - Vai trò trong hệ thống (Admin, Manager, Trainer, Receptionist)
/// </summary>
public class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>
    /// JSON string chứa danh sách permissions
    /// </summary>
    public string? Permissions { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
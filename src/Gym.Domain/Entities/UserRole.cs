using System;

namespace Gym.Domain.Entities;

/// <summary>
/// Junction table for User and Role (Many-to-Many RBAC)
/// </summary>
public class UserRole
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}

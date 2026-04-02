using Gym.Domain.Common;
using Gym.Domain.Enums;
using System;

namespace Gym.Domain.Entities;

/// <summary>
/// Audit Log - "Hộp đen" tối quan trọng của hệ thống phòng gym
/// </summary>
public class AuditLog : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty; // Nguyễn Văn A
    public string UserRole { get; set; } = string.Empty; // Receptionist, Admin...

    public string Action { get; set; } = string.Empty; // inventory.consume, package.update...
    public string EntityName { get; set; } = string.Empty; // Product, MembershipPackage...
    public string? ResourceId { get; set; } // ID của đối tượng
    public string? ResourceName { get; set; } // Ví dụ: Khăn lau, Gói tập VIP...

    public string OldValues { get; set; } = "{}";
    public string NewValues { get; set; } = "{}";

    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; } // Chrome, App...
    public string? Reason { get; set; } // Lý do thay đổi (nếu có)

    public AuditSeverity Severity { get; set; } = AuditSeverity.Normal;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

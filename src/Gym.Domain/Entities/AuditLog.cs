using Gym.Domain.Common;
using System;

namespace Gym.Domain.Entities;

public class AuditLog : BaseEntity // Kế thừa BaseEntity nếu project bạn có dùng
{
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string OldValues { get; set; } = string.Empty;
    public string NewValues { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
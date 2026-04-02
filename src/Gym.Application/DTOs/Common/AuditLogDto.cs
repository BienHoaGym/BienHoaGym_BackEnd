using System;

namespace Gym.Application.DTOs.Common
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string? ResourceId { get; set; }
        public string? ResourceName { get; set; }

        public string OldValues { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;

        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Reason { get; set; }

        public int Severity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

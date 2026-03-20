using System;

namespace Gym.Application.DTOs.Common
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; // Thêm cột Tên người dùng hiển thị
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string OldValues { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
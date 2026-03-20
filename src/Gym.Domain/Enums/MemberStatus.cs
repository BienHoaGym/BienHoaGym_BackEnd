using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.Domain.Enums;

/// <summary>
/// Trạng thái của Member
/// </summary>
public enum MemberStatus
{
    /// <summary>
    /// Hội viên tiềm năng - Đăng ký từ Marketing web, chờ tư vấn
    /// </summary>
    Prospective = 0,

    /// <summary>
    /// Đang hoạt động - Member có thể sử dụng dịch vụ
    /// </summary>
    Active = 1,

    /// <summary>
    /// Không hoạt động - Member tạm thời không sử dụng
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Bị đình chỉ - Vi phạm quy định
    /// </summary>
    Suspended = 3
}
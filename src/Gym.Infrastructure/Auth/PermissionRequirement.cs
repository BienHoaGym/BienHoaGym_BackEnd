using Microsoft.AspNetCore.Authorization;

namespace Gym.Infrastructure.Auth;

/// <summary>
/// Thỏa mãn yêu cầu nếu người dùng có một permission cụ thể
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

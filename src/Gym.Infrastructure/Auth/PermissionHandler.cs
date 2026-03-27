using Microsoft.AspNetCore.Authorization;

namespace Gym.Infrastructure.Auth;

/// <summary>
/// Kiểm tra xem User có Permission claim tương ứng không
/// </summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // 1. Phải đăng nhập
        if (context.User == null) return Task.CompletedTask;

        // 2. Kiểm tra nếu là Admin (Toàn quyền)
        // Lưu ý: ClaimType "Permission" được set trong JwtService.cs
        bool isAdmin = context.User.IsInRole("Admin");
        if (isAdmin)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // 3. Kiểm tra permission cụ thể (Hoặc wildcard '*')
        var hasPermission = context.User.HasClaim(c => c.Type == "Permission" && 
            (c.Value == requirement.Permission || c.Value == "*"));
            
        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Gym.Application.Interfaces.Services;

namespace Gym.Infrastructure.Auth;

/// <summary>
/// Kiểm tra xem User có Permission thực tế trong DB không (Dynamic RBAC)
/// </summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // 1. Phải đăng nhập
        var userIdStr = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId)) 
            return;

        // 2. Sử dụng PermissionService để kiểm tra quyền THỰC TẾ trong DB
        using (var scope = _serviceProvider.CreateScope())
        {
            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
            
            if (await permissionService.UserHasPermissionAsync(userId, requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}

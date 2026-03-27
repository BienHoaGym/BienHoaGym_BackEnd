using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Gym.Infrastructure.Auth;

/// <summary>
/// Tự động tạo chính sách (Policy) cho các permission được yêu cầu
/// Ví dụ: [Authorize(Policy = "member.read")]
/// </summary>
public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Kiểm tra xem đây có phải là policy mặc định (nguồn gốc từ base)
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null) return policy;

        // Nếu không có sẵn, ta coi đây là 1 Permission và tự tạo policy động
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}

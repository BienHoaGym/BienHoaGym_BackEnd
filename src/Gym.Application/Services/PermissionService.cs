using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gym.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return new List<string>();

        var permissions = new HashSet<string>();

        foreach (var userRole in user.UserRoles)
        {
            if (userRole.Role == null || string.IsNullOrEmpty(userRole.Role.Permissions)) continue;

            // Kiểm tra dấu * ở mọi định dạng có thể có trong DB
            if (userRole.Role.Permissions == "*" || userRole.Role.Permissions == "\"*\"" || userRole.Role.Permissions == "[\"*\"]")
            {
                permissions.Add("*");
                continue;
            }

            try
            {
                var rolePerms = JsonSerializer.Deserialize<List<string>>(userRole.Role.Permissions);
                if (rolePerms != null)
                {
                    foreach (var p in rolePerms) permissions.Add(p);
                }
            }
            catch
            {
                // Ignore malformed JSON
            }
        }

        return permissions.ToList();
    }

    public async Task<bool> UserHasPermissionAsync(Guid userId, string requiredPermission)
    {
        var permissions = await GetUserPermissionsAsync(userId);
        
        if (permissions.Contains("*")) return true;

        var parts = requiredPermission.Split('.');
        if (parts.Length < 2) return permissions.Contains(requiredPermission);

        string module = parts[0];

        // 1. Kiểm tra wildcard module (ví dụ: member.*)
        if (permissions.Contains($"{module}.*")) return true;

        // 2. Kiểm tra quyền trực tiếp
        if (permissions.Contains(requiredPermission)) return true;

        return false;
    }
}

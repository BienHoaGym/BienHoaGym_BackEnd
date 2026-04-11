using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces.Services;

public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(Guid userId, string requiredPermission);
    Task<List<string>> GetUserPermissionsAsync(Guid userId);
}

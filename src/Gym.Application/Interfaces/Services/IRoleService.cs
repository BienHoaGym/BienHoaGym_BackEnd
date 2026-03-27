using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Roles;

namespace Gym.Application.Interfaces.Services;

public interface IRoleService
{
    Task<ResponseDto<List<RoleDto>>> GetAllAsync();
    Task<ResponseDto<RoleDto>> GetByIdAsync(int id);
    Task<ResponseDto<RoleDto>> CreateAsync(CreateRoleDto dto);
    Task<ResponseDto<RoleDto>> UpdateAsync(int id, UpdateRoleDto dto);
    Task<ResponseDto<bool>> DeleteAsync(int id);
    Task<ResponseDto<List<string>>> GetAllAvailablePermissions();
}

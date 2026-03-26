using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Roles;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gym.Application.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<RoleDto>>> GetAllAsync()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        var dtos = roles.Select(MapToDto).ToList();
        return ResponseDto<List<RoleDto>>.SuccessResult(dtos);
    }

    public async Task<ResponseDto<RoleDto>> GetByIdAsync(int id)
    {
        var role = await _unitOfWork.Roles.GetQueryable().FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return ResponseDto<RoleDto>.FailureResult("Role not found");
        return ResponseDto<RoleDto>.SuccessResult(MapToDto(role));
    }

    public async Task<ResponseDto<RoleDto>> CreateAsync(CreateRoleDto dto)
    {
        var role = new Role
        {
            RoleName = dto.RoleName,
            Description = dto.Description,
            Permissions = JsonSerializer.Serialize(dto.Permissions),
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<RoleDto>.SuccessResult(MapToDto(role), "Created");
    }

    public async Task<ResponseDto<RoleDto>> UpdateAsync(int id, UpdateRoleDto dto)
    {
        var role = await _unitOfWork.Roles.GetQueryable().FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return ResponseDto<RoleDto>.FailureResult("Role not found");

        role.RoleName = dto.RoleName;
        role.Description = dto.Description;
        role.Permissions = JsonSerializer.Serialize(dto.Permissions);
        
        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<RoleDto>.SuccessResult(MapToDto(role), "Updated");
    }

    public async Task<ResponseDto<bool>> DeleteAsync(int id)
    {
        var role = await _unitOfWork.Roles.GetQueryable().FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return ResponseDto<bool>.FailureResult("Role not found");

        // Không cho xóa Role Admin
        if (role.RoleName == "Admin") return ResponseDto<bool>.FailureResult("Không được xóa vai trò Admin hệ thống.");

        _unitOfWork.Roles.Delete(role);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Deleted");
    }

    public Task<ResponseDto<List<string>>> GetAllAvailablePermissions()
    {
        var perms = typeof(PermissionConstants)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetValue(null)?.ToString() ?? "")
            .ToList();

        return Task.FromResult(ResponseDto<List<string>>.SuccessResult(perms));
    }

    private RoleDto MapToDto(Role role)
    {
        List<string> perms = new();
        if (!string.IsNullOrEmpty(role.Permissions))
        {
            try { perms = JsonSerializer.Deserialize<List<string>>(role.Permissions) ?? new(); }
            catch { }
        }

        return new RoleDto
        {
            Id = role.Id,
            RoleName = role.RoleName,
            Description = role.Description,
            Permissions = perms
        };
    }
}

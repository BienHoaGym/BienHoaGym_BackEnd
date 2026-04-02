using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Gym.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto<List<UserListDto>>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Trainer)
            .ToListAsync();

        var dtos = users.Select(u => new UserListDto
        {
            Id = u.Id,
            Username = u.Username,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToList(),
            RoleIds = u.UserRoles.Select(ur => ur.RoleId).ToList(),
            Role = u.UserRoles.FirstOrDefault()?.Role.RoleName ?? "Không có vai trò",
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt,
            // Trainer fields
            Specialization = u.Trainer?.Specialization,
            ExperienceYears = u.Trainer?.ExperienceYears,
            TrainerCode = u.Trainer?.TrainerCode,
            Salary = u.Trainer?.Salary
        }).ToList();

        return ResponseDto<List<UserListDto>>.SuccessResult(dtos);
    }

    public async Task<ResponseDto<UserListDto>> GetByIdAsync(Guid userId)
    {
        var u = await _unitOfWork.Users.GetQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Trainer)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (u == null) return ResponseDto<UserListDto>.FailureResult("Không tìm thấy người dùng");

        var dto = new UserListDto
        {
            Id = u.Id,
            Username = u.Username,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToList(),
            RoleIds = u.UserRoles.Select(ur => ur.RoleId).ToList(),
            Role = u.UserRoles.FirstOrDefault()?.Role.RoleName ?? "Không có vai trò",
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt,
            // Trainer fields
            Specialization = u.Trainer?.Specialization,
            ExperienceYears = u.Trainer?.ExperienceYears,
            TrainerCode = u.Trainer?.TrainerCode,
            Salary = u.Trainer?.Salary
        };

        return ResponseDto<UserListDto>.SuccessResult(dto);
    }

    public async Task<ResponseDto<bool>> UpdateStaffAsync(Guid userId, UpdateStaffDto dto)
    {
        var user = await _unitOfWork.Users.GetQueryable()
            .Include(u => u.UserRoles)
            .Include(u => u.Trainer)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return ResponseDto<bool>.FailureResult("Không tìm thấy người dùng");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        // Cập nhật roles (Chủ động để tránh lỗi Concurrency khi Clear/Add)
        var currentRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
        
        // Xóa các role không còn trong danh sách mới
        var rolesToRemove = user.UserRoles.Where(ur => !dto.RoleIds.Contains(ur.RoleId)).ToList();
        foreach (var rtr in rolesToRemove)
        {
            user.UserRoles.Remove(rtr);
        }

        // Thêm các role mới
        foreach (var rid in dto.RoleIds)
        {
            if (!currentRoleIds.Contains(rid))
            {
                user.UserRoles.Add(new UserRole { UserId = userId, RoleId = rid });
            }
        }

        // Nếu là Trainer -> Cập nhật thông tin Trainer
        var isTrainer = dto.RoleIds.Contains(3);
        if (isTrainer)
        {
            // Kiểm tra xem đã có bản ghi Trainer chưa (trường hợp Include không ra hoặc mới add)
            var trainer = user.Trainer;
            if (trainer == null)
            {
                // Thử tìm trong DB một lần nữa theo UserId (phòng hờ bản ghi đã tồn tại nhưng IsDeleted=true hoặc lý do khác)
                trainer = await _unitOfWork.Trainers.GetQueryable()
                    .FirstOrDefaultAsync(t => t.UserId == userId);
            }

            if (trainer == null)
            {
                trainer = new Trainer
                {
                    UserId = userId,
                    FullName = dto.FullName,
                    TrainerCode = "PT-" + DateTime.UtcNow.Ticks.ToString().Substring(10),
                    Specialization = dto.Specialization,
                    ExperienceYears = dto.ExperienceYears ?? 0,
                    Salary = dto.Salary ?? 0,
                    IsActive = true
                };
                await _unitOfWork.Trainers.AddAsync(trainer);
                user.Trainer = trainer;
            }
            else
            {
                trainer.FullName = dto.FullName;
                trainer.Specialization = dto.Specialization;
                trainer.ExperienceYears = dto.ExperienceYears ?? 0;
                trainer.Salary = dto.Salary ?? 0;
                trainer.IsActive = dto.IsActive;
                trainer.IsDeleted = false; // Phục hồi nếu lỡ bị xóa mềm
                _unitOfWork.Trainers.Update(trainer);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Cập nhật nhân viên thành công");
    }

    public async Task<ResponseDto<Guid>> CreateStaffAsync(CreateStaffDto dto)
    {
        // Kiểm tra username trùng
        var existing = await _unitOfWork.Users.GetQueryable().AnyAsync(u => u.Username == dto.Username);
        if (existing) return ResponseDto<Guid>.FailureResult("Tên đăng nhập đã tồn tại");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Dùng BCrypt nếu đã cài, hoặc tạm để trống
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var rid in dto.RoleIds)
        {
            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = rid });
        }

        if (dto.RoleIds.Contains(3)) // Role Trainer
        {
            user.Trainer = new Trainer
            {
                UserId = user.Id,
                FullName = dto.FullName,
                TrainerCode = "PT-" + DateTime.UtcNow.Ticks.ToString().Substring(10),
                Specialization = dto.Specialization,
                ExperienceYears = dto.ExperienceYears ?? 0,
                Salary = dto.Salary ?? 0,
                IsActive = true
            };
        }

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<Guid>.SuccessResult(user.Id, "Đã tạo nhân viên");
    }

    public async Task<ResponseDto<bool>> SetUserRolesAsync(Guid userId, List<int> roleIds)
    {
        var user = await _unitOfWork.Users.GetQueryable()
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return ResponseDto<bool>.FailureResult("Không tìm thấy người dùng");

        user.UserRoles.Clear();
        foreach (var rid in roleIds)
        {
            user.UserRoles.Add(new UserRole { UserId = userId, RoleId = rid });
        }

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Đã cập nhật vai trò");
    }
}

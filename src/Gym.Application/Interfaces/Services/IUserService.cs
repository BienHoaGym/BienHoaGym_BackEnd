using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gym.Application.DTOs.Common;

namespace Gym.Application.Interfaces.Services;

public interface IUserService
{
    Task<ResponseDto<List<UserListDto>>> GetAllAsync();
    Task<ResponseDto<UserListDto>> GetByIdAsync(Guid userId);
    Task<ResponseDto<bool>> SetUserRolesAsync(Guid userId, List<int> roleIds);
    Task<ResponseDto<bool>> UpdateStaffAsync(Guid userId, UpdateStaffDto dto);
    Task<ResponseDto<Guid>> CreateStaffAsync(CreateStaffDto dto);
    Task<ResponseDto<bool>> ResetPasswordAsync(Guid userId, string newPassword);
}

public class UserListDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<int> RoleIds { get; set; } = new();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? IdentityNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public DateTime? HireDate { get; set; }
    public string? BankCardNumber { get; set; }
    public string? BankName { get; set; }
    
    // Trainer specific fields
    public string? Specialization { get; set; }
    public int? ExperienceYears { get; set; }
    public string? TrainerCode { get; set; }
    public decimal? Salary { get; set; }
}

public class CreateStaffDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<int> RoleIds { get; set; } = new();

    public string? IdentityNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public DateTime? HireDate { get; set; }
    public string? BankCardNumber { get; set; }
    public string? BankName { get; set; }
    
    // Trainer info if applicable
    public string? Specialization { get; set; }
    public int? ExperienceYears { get; set; }
    public decimal? Salary { get; set; }
}

public class UpdateStaffDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<int> RoleIds { get; set; } = new();
    public bool IsActive { get; set; }

    public string? IdentityNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public DateTime? HireDate { get; set; }
    public string? BankCardNumber { get; set; }
    public string? BankName { get; set; }
    
    // Trainer info
    public string? Specialization { get; set; }
    public int? ExperienceYears { get; set; }
    public decimal? Salary { get; set; }
}

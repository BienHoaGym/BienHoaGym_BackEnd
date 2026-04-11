using Gym.Application.DTOs.Auth;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Users;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gym.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    // Đã xóa: private readonly IMapper _mapper;  <-- Không cần nữa
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly int _tokenExpiryMinutes;

    public AuthService(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        // Đã xóa: IMapper mapper, <-- Không inject nữa
        IPasswordHasher<User> passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        // _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenExpiryMinutes = 60; // Thời gian hết hạn token (phút)
    }

    public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        // 1. Tìm user
        var user = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);

        if (user == null)
        {
            return ResponseDto<LoginResponseDto>.FailureResult("Tên đăng nhập hoặc mật khẩu không chính xác");
        }

        // 2. So khớp mật khẩu (Hỗ trợ cả BCrypt và Legacy Identity Hash)
        bool isPasswordValid = false;
        
        // Thử BCrypt trước (Cho các tài khoản mới hoặc vừa Reset)
        try 
        {
            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                isPasswordValid = true;
            }
        }
        catch 
        {
            // Nếu không phải định dạng BCrypt, thử dùng PasswordHasher mặc định
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verificationResult != PasswordVerificationResult.Failed)
            {
                isPasswordValid = true;
            }
        }

        if (!isPasswordValid)
        {
            return ResponseDto<LoginResponseDto>.FailureResult("Tên đăng nhập hoặc mật khẩu không chính xác");
        }

        // 3. Kiểm tra Active
        if (!user.IsActive)
        {
            return ResponseDto<LoginResponseDto>.FailureResult("Tài khoản đã bị tạm khóa");
        }

        // 4. Tạo Token
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // 5. Cập nhật lần đăng nhập cuối
        user.LastLoginAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        // ====================================================================
        // KHU VỰC SỬA LỖI: MAP THỦ CÔNG (MANUAL MAPPING)
        // Thay vì dùng _mapper.Map, ta tự gán từng trường một.
        // ====================================================================
        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        var permissionsSet = new HashSet<string>();
        foreach (var ur in user.UserRoles)
        {
            if (ur.Role == null) continue;

            // 1. Ưu tiên dấu * (Toàn quyền) nếu role là Admin hoặc có dấu * trong DB
            if (ur.Role.RoleName == "Admin" || 
                ur.Role.Permissions == "*" || 
                ur.Role.Permissions == "\"*\"" || 
                ur.Role.Permissions == "[\"*\"]")
            {
                permissionsSet.Add("*");
            }

            // 2. Nạp thêm các quyền cụ thể khác
            if (!string.IsNullOrEmpty(ur.Role.Permissions) && ur.Role.Permissions != "*")
            {
                try
                {
                    var permslist = System.Text.Json.JsonSerializer.Deserialize<List<string>>(ur.Role.Permissions);
                    if (permslist != null)
                    {
                        foreach (var p in permslist) permissionsSet.Add(p);
                    }
                }
                catch { }
            }
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            Roles = roles,
            Permissions = permissionsSet.ToList(),
            Role = roles.Contains("Admin") ? "Admin" : (roles.FirstOrDefault() ?? string.Empty)
        };

        var response = new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenExpiryMinutes),
            User = userDto // Gán đối tượng vừa tạo vào đây
        };

        return ResponseDto<LoginResponseDto>.SuccessResult(response, "Đăng nhập thành công");
    }

    public Task<ResponseDto<bool>> LogoutAsync(Guid userId)
    {
        return Task.FromResult(ResponseDto<bool>.SuccessResult(true, "Đăng xuất thành công"));
    }

    public async Task<List<string>> GetPermissionsByRoleIdsAsync(List<int> roleIds)
    {
        var permissions = new HashSet<string>();
        var roles = _unitOfWork.Roles.GetQueryable().Where(r => roleIds.Contains(r.Id)).ToList();
        
        foreach (var r in roles)
        {
            if (string.IsNullOrEmpty(r.Permissions)) continue;

            // 1. Kiểm tra dấu * (Toàn quyền) nếu role là Admin hoặc có dấu * trong DB
            if (r.RoleName == "Admin" || r.Permissions == "*" || r.Permissions == "\"*\"" || r.Permissions == "[\"*\"]")
            {
                permissions.Add("*");
            }

            // 2. Nạp thêm các quyền cụ thể
            if (r.Permissions != "*")
            {
                try
                {
                    var perms = System.Text.Json.JsonSerializer.Deserialize<List<string>>(r.Permissions);
                    if (perms != null)
                    {
                        foreach (var p in perms) permissions.Add(p);
                    }
                }
                catch { /* Ignore invalid JSON */ }
            }
        }
        
        return permissions.ToList();
    }
}

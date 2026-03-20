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
            return ResponseDto<LoginResponseDto>.FailureResult("Invalid username or password");
        }

        // 2. So khớp mật khẩu (Dùng Hash)
        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return ResponseDto<LoginResponseDto>.FailureResult("Invalid username or password");
        }

        // 3. Kiểm tra Active
        if (!user.IsActive)
        {
            return ResponseDto<LoginResponseDto>.FailureResult("Account is inactive");
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
        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,

            // Xử lý Role: Lấy tên Role từ bảng Role (nếu có), nếu null thì để chuỗi rỗng
            // Lưu ý: Đảm bảo hàm GetByUsernameAsync của bạn có .Include(u => u.Role)
            Role = user.Role != null ? user.Role.RoleName : string.Empty
        };

        var response = new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenExpiryMinutes),
            User = userDto // Gán đối tượng vừa tạo vào đây
        };

        return ResponseDto<LoginResponseDto>.SuccessResult(response, "Login successful");
    }

    public async Task<ResponseDto<bool>> LogoutAsync(Guid userId)
    {
        return ResponseDto<bool>.SuccessResult(true, "Logout successful");
    }
}
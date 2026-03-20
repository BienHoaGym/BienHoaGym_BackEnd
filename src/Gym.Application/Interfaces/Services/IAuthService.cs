using Gym.Application.DTOs.Auth;
using Gym.Application.DTOs.Common;
using Gym.Domain.Entities;
namespace Gym.Application.Interfaces.Services;

public interface IAuthService
{
    Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
    Task<ResponseDto<bool>> LogoutAsync(Guid userId);
}
using Gym.Domain.Entities;
using System.Security.Claims;

namespace Gym.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}
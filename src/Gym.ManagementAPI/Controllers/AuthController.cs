    using Gym.Application.DTOs.Auth;
    using Gym.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    namespace Gym.ManagementAPI.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Login to get access token
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Login attempt for user: {Username}", dto.Username);

            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                _logger.LogWarning("Login failed for user: {Username}", dto.Username);
                return Unauthorized(result);
            }

            _logger.LogInformation("Login successful for user: {Username}", dto.Username);
            return Ok(result);
        }

        /// <summary>
        /// Logout (invalidate token)
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            _logger.LogInformation("Logout request for user: {UserId}", userId);

            var result = await _authService.LogoutAsync(Guid.Parse(userId));

            return Ok(result);
        }

        /// <summary>
        /// Get current user info - Lấy trực tiếp từ DB để đảm bảo quyền luôn mới
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser([FromServices] IUserService userService)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var result = await userService.GetByIdAsync(userId);
            if (!result.Success) return NotFound(result);

            var user = result.Data;

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    fullName = user.FullName,
                    role = user.Role,
                    roles = user.Roles,
                    permissions = user.RoleIds != null ? await _authService.GetPermissionsByRoleIdsAsync(user.RoleIds) : new List<string>()
                }
            });
        }
    }

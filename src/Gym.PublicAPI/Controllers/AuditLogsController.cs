using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bắt buộc đăng nhập
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AuditLogsController> _logger;
    public AuditLogsController(IAuditLogService auditLogService, ILogger<AuditLogsController> logger)
    {
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách lịch sử thao tác hệ thống (Chỉ Admin và Manager)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")] 
    public async Task<IActionResult> GetLogs([FromQuery] string? userId = null, [FromQuery] int? severity = null, [FromQuery] string? action = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        _logger.LogInformation("Lấy danh sách nhật ký hệ thống: UserId={userId}, Action={action}", userId, action);
        var result = await _auditLogService.GetAllAsync(userId, severity, action, fromDate, toDate);
        return Ok(result);
    }
}

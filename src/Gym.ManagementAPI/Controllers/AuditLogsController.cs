using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bắt buộc đăng nhập
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AuditLogsController> _logger;
    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Lấy danh sách lịch sử thao tác hệ thống (Chỉ Admin và Manager)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")] // Phân quyền cực kỳ chặt chẽ
    public async Task<IActionResult> GetLogs()
    {
        var result = await _auditLogService.GetAllAsync();
        return Ok(result);
    }
}
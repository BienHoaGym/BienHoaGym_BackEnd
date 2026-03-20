using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Manager")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var result = await _reportsService.GetRevenueReportAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("assets-inventory")]
    public async Task<IActionResult> GetAssetInventoryStats()
    {
        var result = await _reportsService.GetAssetInventoryStatsAsync();
        return Ok(result);
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedData()
    {
        var result = await _reportsService.SeedReportDataAsync();
        return Ok(result);
    }
}

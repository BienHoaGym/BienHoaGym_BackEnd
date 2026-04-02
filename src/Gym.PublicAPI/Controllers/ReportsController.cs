using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("revenue")]
    [Authorize(Policy = PermissionConstants.ReportFinancial)]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var result = await _reportsService.GetRevenueReportAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("assets-inventory")]
    [Authorize(Policy = PermissionConstants.ReportRead)]
    public async Task<IActionResult> GetAssetInventoryStats()
    {
        var result = await _reportsService.GetAssetInventoryStatsAsync();
        return Ok(result);
    }

    [HttpGet("operating-costs")]
    [Authorize(Policy = PermissionConstants.ReportFinancial)]
    public async Task<IActionResult> GetOperatingCostReport([FromQuery] int month, [FromQuery] int year)
    {
        var result = await _reportsService.GetOperatingCostReportAsync(month, year);
        return Ok(result);
    }

    [HttpPost("seed")]
    [Authorize(Policy = PermissionConstants.SettingsManage)]
    public async Task<IActionResult> SeedData()
    {
        var result = await _reportsService.SeedReportDataAsync();
        return Ok(result);
    }
}

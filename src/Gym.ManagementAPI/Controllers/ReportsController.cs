using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Gym.Application.DTOs.Common;
using Gym.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

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

    [HttpGet("revenue/export")]
    [Authorize(Policy = PermissionConstants.ReportFinancial)]
    public async Task<IActionResult> ExportRevenueExcel([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var fileContent = await _reportsService.ExportRevenueToExcelAsync(startDate, endDate);
        string fileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd}.xlsx";
        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
        try {
            await DataSeeder.SeedDemoDataAsync(HttpContext.RequestServices);
            return Ok(ResponseDto<bool>.SuccessResult(true, "D\u1EEF li\u1EC7u \u0111\u00E3 \u0111\u01B0\u1EE3c n\u1EA1p th\u00E0nh c\u00F4ng."));
        } catch (Exception ex) {
            return BadRequest(ResponseDto<bool>.FailureResult($"L\u1ED7i seed: {ex.Message}"));
        }
    }
}

using Gym.Application.DTOs.Equipment;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.Domain.Enums;
using Gym.Domain.Constants;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEquipments(
        [FromQuery] Guid? categoryId, 
        [FromQuery] Guid? providerId, 
        [FromQuery] EquipmentStatus? status, 
        [FromQuery] string? location, 
        [FromQuery] string? searchTerm)
    {
        var result = await _equipmentService.GetEquipmentsAsync(categoryId, providerId, status, location, searchTerm);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _equipmentService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionConstants.EquipmentCreate)]
    public async Task<IActionResult> Create([FromBody] CreateEquipmentDto dto)
    {
        var result = await _equipmentService.CreateEquipmentAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PermissionConstants.EquipmentUpdate)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateEquipmentDto dto)
    {
        var result = await _equipmentService.UpdateEquipmentAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionConstants.EquipmentDelete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _equipmentService.DeleteEquipmentAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/liquidate")]
    [Authorize(Policy = PermissionConstants.EquipmentUpdate)]
    public async Task<IActionResult> Liquidate(Guid id)
    {
        var result = await _equipmentService.LiquidateEquipmentAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/provider-history")]
    public async Task<IActionResult> GetProviderHistory(Guid id)
    {
        var result = await _equipmentService.GetProviderHistoryAsync(id);
        return Ok(result);
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] Guid? equipmentId)
    {
        var result = await _equipmentService.GetTransactionsAsync(equipmentId);
        return Ok(result);
    }

    [HttpPost("transactions")]
    public async Task<IActionResult> RecordTransaction([FromBody] CreateEquipmentTransactionDto dto)
    {
        var result = await _equipmentService.RecordTransactionAsync(dto);
        return Ok(result);
    }

    [HttpGet("maintenance")]
    public async Task<IActionResult> GetMaintenanceLogs([FromQuery] Guid? equipmentId)
    {
        var result = await _equipmentService.GetMaintenanceLogsAsync(equipmentId);
        return Ok(result);
    }

    [HttpPost("maintenance")]
    public async Task<IActionResult> LogMaintenance([FromBody] CreateMaintenanceLogDto dto)
    {
        var result = await _equipmentService.LogMaintenanceAsync(dto);
        return Ok(result);
    }

    [HttpGet("incidents")]
    public async Task<IActionResult> GetIncidentLogs([FromQuery] Guid? equipmentId)
    {
        var result = await _equipmentService.GetIncidentLogsAsync(equipmentId);
        return Ok(result);
    }

    [HttpPost("incidents")]
    public async Task<IActionResult> LogIncident([FromBody] CreateIncidentLogDto dto)
    {
        var result = await _equipmentService.LogIncidentAsync(dto);
        return Ok(result);
    }

    [HttpGet("depreciations")]
    public async Task<IActionResult> GetDepreciations([FromQuery] Guid? equipmentId)
    {
        var result = await _equipmentService.GetDepreciationsAsync(equipmentId);
        return Ok(result);
    }

    [HttpGet("maintenance/plan")]
    public async Task<IActionResult> GetMaintenancePlan()
    {
        var result = await _equipmentService.GetMaintenancePlanAsync();
        return Ok(result);
    }

    [HttpPost("{id}/depreciation")]
    [Authorize(Policy = PermissionConstants.ReportFinancial)]
    public async Task<IActionResult> RecordDepreciation(Guid id, [FromQuery] int month, [FromQuery] int year, [FromQuery] string? note)
    {
        var result = await _equipmentService.RecordDepreciationAsync(id, month, year, note);
        return Ok(result);
    }

    [HttpPost("bulk-depreciation")]
    [Authorize(Policy = PermissionConstants.ReportFinancial)]
    public async Task<IActionResult> BulkRecordDepreciation([FromQuery] int month, [FromQuery] int year)
    {
        var result = await _equipmentService.BulkRecordDepreciationAsync(month, year);
        return Ok(result);
    }
}

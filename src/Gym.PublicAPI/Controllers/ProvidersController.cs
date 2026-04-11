using Gym.Application.DTOs.Providers;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.Domain.Constants;
using System;
using System.Threading.Tasks;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionConstants.ProviderRead)]
public class ProvidersController : ControllerBase
{
    private readonly IProviderService _providerService;

    public ProvidersController(IProviderService providerService)
    {
        _providerService = providerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null, [FromQuery] string? supplyType = null)
    {
        var result = await _providerService.GetAllSummariesAsync(searchTerm, supplyType);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _providerService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory(Guid id)
    {
        var result = await _providerService.GetTransactionHistoryAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionConstants.SettingsManage)]
    public async Task<IActionResult> Create(CreateProviderDto dto)
    {
        var result = await _providerService.CreateAsync(dto);
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PermissionConstants.SettingsManage)]
    public async Task<IActionResult> Update(Guid id, CreateProviderDto dto)
    {
        var result = await _providerService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionConstants.SettingsManage)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _providerService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetProducts(Guid id)
    {
        var result = await _providerService.GetProductsAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/equipments")]
    public async Task<IActionResult> GetEquipments(Guid id)
    {
        var result = await _providerService.GetEquipmentsAsync(id);
        return Ok(result);
    }

    [HttpPost("pay-debt")]
    [Authorize(Policy = PermissionConstants.BillingCreate)]
    public async Task<IActionResult> PayDebt(CreateProviderPaymentDto dto)
    {
        var result = await _providerService.PayDebtAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}

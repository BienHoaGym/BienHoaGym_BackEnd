using Gym.Application.DTOs.Providers;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> Create(CreateProviderDto dto)
    {
        var result = await _providerService.CreateAsync(dto);
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CreateProviderDto dto)
    {
        var result = await _providerService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _providerService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}

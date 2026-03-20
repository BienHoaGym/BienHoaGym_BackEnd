using Gym.Application.DTOs.Billing;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;

    public BillingController(IBillingService billingService)
    {
        _billingService = billingService;
    }

    // Products
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _billingService.GetProductsAsync();
        return Ok(result);
    }

    [HttpPost("products")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        var result = await _billingService.CreateProductAsync(dto);
        return Ok(result);
    }

    // Invoices
    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices()
    {
        var result = await _billingService.GetInvoicesAsync();
        return Ok(result);
    }

    [HttpGet("invoices/{id}")]
    public async Task<IActionResult> GetInvoice(Guid id)
    {
        var result = await _billingService.GetInvoiceByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost("invoices")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
    {
        var result = await _billingService.CreateInvoiceAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

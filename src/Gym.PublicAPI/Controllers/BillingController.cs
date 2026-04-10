using Gym.Application.DTOs.Billing;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

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
    [Authorize(Policy = PermissionConstants.ProductCreate)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        var result = await _billingService.CreateProductAsync(dto);
        return Ok(result);
    }

    // Invoices
    [HttpGet("invoices")]
    [Authorize(Policy = PermissionConstants.BillingRead)]
    public async Task<IActionResult> GetInvoices()
    {
        var result = await _billingService.GetInvoicesAsync();
        return Ok(result);
    }

    [HttpGet("invoices/{id}")]
    [Authorize(Policy = PermissionConstants.BillingRead)]
    public async Task<IActionResult> GetInvoice(Guid id)
    {
        var result = await _billingService.GetInvoiceByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost("invoices")]
    [Authorize(Policy = PermissionConstants.BillingCreate)]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
    {
        var result = await _billingService.CreateInvoiceAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("invoices/{id}/pdf")]
    [Authorize(Policy = PermissionConstants.BillingRead)]
    public async Task<IActionResult> DownloadInvoicePdf(Guid id)
    {
        var fileContent = await _billingService.ExportInvoicePdfAsync(id);
        if (fileContent == null || fileContent.Length == 0) return NotFound("Kh\u00F4ng th\u1EC3 t\u1EA1o PDF");
        
        string fileName = $"HoaDon_{id}.pdf";
        return File(fileContent, "application/pdf", fileName);
    }
}

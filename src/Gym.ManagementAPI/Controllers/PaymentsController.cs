using Gym.Application.DTOs.Payments;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = PermissionConstants.PaymentRead)]
    public async Task<IActionResult> GetPayments()
    {
        var result = await _paymentService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PermissionConstants.PaymentRead)]
    public async Task<IActionResult> GetPayment(Guid id)
    {
        var result = await _paymentService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("subscription/{subscriptionId}")]
    [Authorize(Policy = PermissionConstants.PaymentRead)]
    public async Task<IActionResult> GetSubscriptionPayments(Guid subscriptionId)
    {
        var result = await _paymentService.GetBySubscriptionIdAsync(subscriptionId);
        return Ok(result);
    }

    [HttpPost("process")]
    [Authorize(Policy = PermissionConstants.PaymentCreate)]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDto dto)
    {
        var result = await _paymentService.ProcessPaymentAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("{id}/refund")]
    [Authorize(Policy = PermissionConstants.PaymentCreate)]
    public async Task<IActionResult> RefundPayment(Guid id, [FromBody] RefundPaymentRequest request)
    {
        var result = await _paymentService.RefundAsync(id, request.Reason);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

public class RefundPaymentRequest
{
    public string Reason { get; set; } = string.Empty;
}

using Gym.Application.DTOs.Subscriptions;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionsController> _logger;

    public SubscriptionsController(ISubscriptionService subscriptionService, ILogger<SubscriptionsController> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = PermissionConstants.SubscriptionRead)]
    public async Task<IActionResult> GetSubscriptions()
    {
        var result = await _subscriptionService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PermissionConstants.SubscriptionRead)]
    public async Task<IActionResult> GetSubscription(Guid id)
    {
        var result = await _subscriptionService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("{id}/detail")]
    [Authorize(Policy = PermissionConstants.SubscriptionRead)]
    public async Task<IActionResult> GetSubscriptionDetail(Guid id)
    {
        var result = await _subscriptionService.GetDetailAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("member/{memberId}")]
    [Authorize(Policy = PermissionConstants.SubscriptionRead)]
    public async Task<IActionResult> GetMemberSubscriptions(Guid memberId)
    {
        var result = await _subscriptionService.GetByMemberIdAsync(memberId);
        return Ok(result);
    }

    [HttpGet("expiring")]
    [Authorize(Policy = PermissionConstants.SubscriptionRead)]
    public async Task<IActionResult> GetExpiringSubscriptions([FromQuery] int days = 7)
    {
        var result = await _subscriptionService.GetExpiringAsync(days);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionConstants.SubscriptionCreate)]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto)
    {
        var result = await _subscriptionService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetSubscription), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}/activate")]
    [Authorize(Policy = PermissionConstants.SubscriptionUpdate)]
    public async Task<IActionResult> ActivateSubscription(Guid id)
    {
        var result = await _subscriptionService.ActivateAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // --- FIX LỖI Ở ĐÂY: Dùng DTO cho Cancel ---
    [HttpPut("{id}/cancel")]
    [Authorize(Policy = PermissionConstants.SubscriptionUpdate)]
    public async Task<IActionResult> CancelSubscription(Guid id, [FromBody] CancelSubscriptionRequest request)
    {
        var result = await _subscriptionService.CancelAsync(id, request.Reason);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("{id}/renew")]
    [Authorize(Policy = PermissionConstants.SubscriptionUpdate)]
    public async Task<IActionResult> RenewSubscription(Guid id, [FromBody] RenewSubscriptionRequest request)
    {
        var result = await _subscriptionService.RenewAsync(id, request.PackageId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}/pause")]
    [Authorize(Policy = PermissionConstants.SubscriptionUpdate)]
    public async Task<IActionResult> PauseSubscription(Guid id, [FromQuery] int? durationDays)
    {
        var result = await _subscriptionService.PauseAsync(id, durationDays);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}/resume")]
    [Authorize(Policy = PermissionConstants.SubscriptionUpdate)]
    public async Task<IActionResult> ResumeSubscription(Guid id)
    {
        var result = await _subscriptionService.ResumeAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

public class RenewSubscriptionRequest
{
    public Guid PackageId { get; set; }
}

// Thêm DTO nhỏ này vào cùng file hoặc file riêng
public class CancelSubscriptionRequest
{
    public string Reason { get; set; } = string.Empty;
}

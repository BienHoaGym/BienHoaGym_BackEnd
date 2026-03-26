using Gym.Application.DTOs.CheckIns;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CheckInsController : ControllerBase
{
    private readonly ICheckInService _checkInService;
    private readonly ILogger<CheckInsController> _logger;

    public CheckInsController(ICheckInService checkInService, ILogger<CheckInsController> logger)
    {
        _checkInService = checkInService;
        _logger = logger;
    }

    /// <summary>
    /// Validate if member can check-in
    /// </summary>
    [HttpPost("validate")]
    [Authorize(Policy = PermissionConstants.CheckInRead)]
    public async Task<IActionResult> ValidateCheckIn([FromBody] ValidateCheckInRequest request)
    {
        var result = await _checkInService.ValidateCheckInAsync(request.MemberCode);
        return Ok(result);
    }

    /// <summary>
    /// Check-in member
    /// </summary>
    [HttpPost]
    [Authorize(Policy = PermissionConstants.CheckInCreate)]
    public async Task<IActionResult> CheckIn([FromBody] CreateCheckInDto dto)
    {
        _logger.LogInformation("Check-in request for member: {MemberCode}", dto.MemberCode);

        var result = await _checkInService.CheckInAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Check-in via QR Code
    /// </summary>
    [HttpPost("qr")]
    [Authorize(Policy = PermissionConstants.CheckInCreate)]
    public async Task<IActionResult> CheckInWithQRCode([FromBody] string qrCode)
    {
        _logger.LogInformation("QR Check-in request");
        var result = await _checkInService.CheckInWithQRCodeAsync(qrCode);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Check-in via Face Recognition
    /// </summary>
    [HttpPost("face")]
    [Authorize(Policy = PermissionConstants.CheckInCreate)]
    public async Task<IActionResult> CheckInWithFace([FromBody] FaceCheckInDto dto)
    {
        _logger.LogInformation("Face Check-in request");
        var result = await _checkInService.CheckInWithFaceAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Check-out member
    /// </summary>
    [HttpPut("{id}/checkout")]
    [Authorize(Policy = PermissionConstants.CheckInCreate)]
    public async Task<IActionResult> CheckOut(Guid id)
    {
        _logger.LogInformation("Check-out request for check-in: {CheckInId}", id);

        var result = await _checkInService.CheckOutAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get today's check-ins
    /// </summary>
    [HttpGet("today")]
    [Authorize(Policy = PermissionConstants.CheckInRead)]
    public async Task<IActionResult> GetTodayCheckIns()
    {
        _logger.LogInformation("Getting today's check-ins");

        var result = await _checkInService.GetTodayCheckInsAsync();

        return Ok(result);
    }

    /// <summary>
    /// Get member check-in history
    /// </summary>
    [HttpGet("member/{memberId}")]
    [Authorize(Policy = PermissionConstants.CheckInRead)]
    public async Task<IActionResult> GetMemberHistory(Guid memberId, [FromQuery] int take = 10)
    {
        _logger.LogInformation("Getting check-in history for member: {MemberId}", memberId);

        var result = await _checkInService.GetMemberHistoryAsync(memberId, take);

        return Ok(result);
    }
}
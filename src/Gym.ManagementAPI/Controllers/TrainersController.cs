using Gym.Application.DTOs.Trainers;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.Domain.Constants;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainersController : ControllerBase
{
    private readonly ITrainerService _trainerService;
    private readonly ILogger<TrainersController> _logger;

    public TrainersController(ITrainerService trainerService, ILogger<TrainersController> logger)
    {
        _trainerService = trainerService;
        _logger = logger;
    }

    [HttpGet("me/schedule")]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> GetMySchedule()
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var fullName = User.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value;
        var isAdmin = User.IsInRole("Admin");

        var result = await _trainerService.GetPersonalScheduleAsync(userId, email, fullName, isAdmin);
        
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("summary/schedule")]
    [Authorize(Roles = "Admin,Manager,Receptionist")]
    public async Task<IActionResult> GetGlobalSchedule()
    {
        var result = await _trainerService.GetGlobalScheduleAsync();
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("{id:guid}/schedule")]
    [Authorize(Roles = "Admin,Manager,Receptionist")]
    public async Task<IActionResult> GetTrainerSchedule(Guid id)
    {
        var result = await _trainerService.GetTrainerScheduleAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get all trainers
    /// URL: GET /api/Trainers
    /// </summary>
    [HttpGet]
    [Authorize(Policy = PermissionConstants.TrainerRead)]
    public async Task<IActionResult> GetTrainers()
    {
        _logger.LogInformation("Getting all trainers");
        var result = await _trainerService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get trainer by ID
    /// URL: GET /api/Trainers/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionConstants.TrainerRead)]
    public async Task<IActionResult> GetTrainer(Guid id)
    {
        _logger.LogInformation("Getting trainer by ID: {TrainerId}", id);
        var result = await _trainerService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get only active trainers
    /// URL: GET /api/Trainers/active
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveTrainers()
    {
        _logger.LogInformation("Getting active trainers");
        var result = await _trainerService.GetAvailableAsync();
        return Ok(result);
    }

    /// <summary>
    /// Create new trainer
    /// URL: POST /api/Trainers
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CreateTrainer([FromBody] CreateTrainerDto dto)
    {
        _logger.LogInformation("Creating new trainer: {FullName}", dto.FullName);
        var result = await _trainerService.CreateAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetTrainer), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Update trainer
    /// URL: PUT /api/Trainers/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateTrainer(Guid id, [FromBody] UpdateTrainerDto dto)
    {
        _logger.LogInformation("Updating trainer: {TrainerId}", id);
        var result = await _trainerService.UpdateAsync(id, dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete trainer (soft delete)
    /// URL: DELETE /api/Trainers/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTrainer(Guid id)
    {
        _logger.LogInformation("Deleting trainer: {TrainerId}", id);
        var result = await _trainerService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id:guid}/members")]
    [Authorize(Policy = PermissionConstants.TrainerRead)]
    public async Task<IActionResult> GetAssignedMembers(Guid id)
    {
        var result = await _trainerService.GetAssignedMembersAsync(id);
        return Ok(result);
    }

    [HttpPost("assign")]
    [Authorize(Roles = "Admin,Manager,Receptionist")]
    public async Task<IActionResult> AssignMember([FromBody] CreateTrainerAssignmentDto dto)
    {
        var result = await _trainerService.AssignMemberAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("unassign/{assignmentId:guid}")]
    [Authorize(Roles = "Admin,Manager,Receptionist")]
    public async Task<IActionResult> RemoveAssignment(Guid assignmentId)
    {
        var result = await _trainerService.RemoveAssignmentAsync(assignmentId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
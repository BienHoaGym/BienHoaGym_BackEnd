using Gym.Application.DTOs.Classes;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.Domain.Constants;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly IClassService _classService;
    private readonly ILogger<ClassesController> _logger;

    public ClassesController(IClassService classService, ILogger<ClassesController> logger)
    {
        _classService = classService;
        _logger = logger;
    }

    /// <summary>
    /// Get all classes
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetClasses()
    {
        _logger.LogInformation("Getting all classes");

        var result = await _classService.GetAllAsync();

        return Ok(result);
    }

    /// <summary>
    /// Get class by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetClass(Guid id)
    {
        _logger.LogInformation("Getting class by ID: {ClassId}", id);

        var result = await _classService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get only active classes
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveClasses()
    {
        _logger.LogInformation("Getting active classes");

        var result = await _classService.GetActiveClassesAsync();

        return Ok(result);
    }

    /// <summary>
    /// Create new class
    /// </summary>
    [HttpPost]
    [Authorize(Policy = PermissionConstants.ClassManage)]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassDto dto)
    {
        _logger.LogInformation("Creating new class: {ClassName}", dto.ClassName);

        var result = await _classService.CreateAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetClass), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Update class
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = PermissionConstants.ClassManage)]
    public async Task<IActionResult> UpdateClass(Guid id, [FromBody] UpdateClassDto dto)
    {
        _logger.LogInformation("Updating class: {ClassId}", id);

        var result = await _classService.UpdateAsync(id, dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete class (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionConstants.ClassManage)]
    public async Task<IActionResult> DeleteClass(Guid id)
    {
        _logger.LogInformation("Deleting class: {ClassId}", id);

        var result = await _classService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Enroll member to class
    /// </summary>
    [HttpPost("{id}/enroll")]
    [Authorize(Policy = PermissionConstants.ClassManage)]
    public async Task<IActionResult> EnrollMember(Guid id, [FromBody] EnrollClassDto dto)
    {
        _logger.LogInformation("Enrolling member {MemberId} to class {ClassId}", dto.MemberId, id);

        var result = await _classService.EnrollMemberAsync(id, dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Unenroll member from class
    /// </summary>
    [HttpPost("{classId}/unenroll/{memberId}")]
    [Authorize(Policy = PermissionConstants.ClassManage)]
    public async Task<IActionResult> UnenrollMember(Guid classId, Guid memberId)
    {
        _logger.LogInformation("Unenrolling member {MemberId} from class {ClassId}", memberId, classId);

        var result = await _classService.UnenrollMemberAsync(classId, memberId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get all enrollments for a class (for attendance)
    /// </summary>
    [HttpGet("{id}/enrollments")]
    [Authorize(Policy = PermissionConstants.ClassRead)]
    public async Task<IActionResult> GetEnrollments(Guid id)
    {
        var result = await _classService.GetEnrollmentsAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Mark attendance for a member in a class
    /// </summary>
    [HttpPost("attendance")]
    [Authorize(Policy = PermissionConstants.ClassManage)]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceDto dto)
    {
        var result = await _classService.MarkAttendanceAsync(dto);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }
}

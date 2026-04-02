using Gym.Application.DTOs.Members;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly ILogger<MembersController> _logger;

    public MembersController(IMemberService memberService, ILogger<MembersController> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = PermissionConstants.MemberRead)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _memberService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PermissionConstants.MemberRead)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _memberService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionConstants.MemberCreate)]
    public async Task<IActionResult> Create([FromBody] CreateMemberDto dto)
    {
        var result = await _memberService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PermissionConstants.MemberUpdate)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberDto dto)
    {
        // Kiểm tra tính nhất quán của ID
        if (dto.Id != Guid.Empty && id != dto.Id)
        {
            return BadRequest("ID trong URL và Body không khớp");
        }

        var result = await _memberService.UpdateAsync(id, dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionConstants.MemberDelete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _memberService.DeleteAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("search")]
    [Authorize(Policy = PermissionConstants.MemberRead)]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _memberService.SearchAsync(keyword);
        return Ok(result);
    }
}

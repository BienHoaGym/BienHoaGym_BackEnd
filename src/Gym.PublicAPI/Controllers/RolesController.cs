using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Roles;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionConstants.RoleRead)]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<List<RoleDto>>>> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto<RoleDto>>> GetById(int id)
    {
        var result = await _roleService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto<RoleDto>>> Create(CreateRoleDto dto)
    {
        var result = await _roleService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseDto<RoleDto>>> Update(int id, UpdateRoleDto dto)
    {
        var result = await _roleService.UpdateAsync(id, dto);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseDto<bool>>> Delete(int id)
    {
        var result = await _roleService.DeleteAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("permissions")]
    public async Task<ActionResult<ResponseDto<List<string>>>> GetAvailablePermissions()
    {
        var result = await _roleService.GetAllAvailablePermissions();
        return Ok(result);
    }
}

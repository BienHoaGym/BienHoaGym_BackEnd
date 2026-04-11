using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionConstants.UserRead)]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<List<UserListDto>>>> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto<UserListDto>>> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto<Guid>>> Create(CreateStaffDto dto)
    {
        var result = await _userService.CreateStaffAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseDto<bool>>> Update(Guid id, UpdateStaffDto dto)
    {
        var result = await _userService.UpdateStaffAsync(id, dto);
        return Ok(result);
    }

    [HttpPost("{id}/roles")]
    public async Task<ActionResult<ResponseDto<bool>>> SetRoles(Guid id, [FromBody] List<int> roleIds)
    {
        var result = await _userService.SetUserRolesAsync(id, roleIds);
        return Ok(result);
    }
}

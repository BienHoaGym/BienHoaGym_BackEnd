using Gym.Application.DTOs.Members;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // Cho phép truy cập không cần token từ Marketing Web
public class PublicController : ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly IClassService _classService;
    private readonly IMemberService _memberService;
    private readonly ITrainerService _trainerService;

    public PublicController(
        IPackageService packageService, 
        IClassService classService,
        IMemberService memberService,
        ITrainerService trainerService)
    {
        _packageService = packageService;
        _classService = classService;
        _memberService = memberService;
        _trainerService = trainerService;
    }

    [HttpGet("packages")]
    public async Task<IActionResult> GetPackages()
    {
        var result = await _packageService.GetActivePackagesAsync();
        return Ok(result);
    }

    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses()
    {
        var result = await _classService.GetActiveClassesAsync();
        return Ok(result);
    }

    [HttpGet("trainers")]
    public async Task<IActionResult> GetTrainers()
    {
        var result = await _trainerService.GetAvailableAsync();
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterLeadDto dto)
    {
        var result = await _memberService.RegisterOnlineAsync(dto);
        return Ok(result);
    }
}

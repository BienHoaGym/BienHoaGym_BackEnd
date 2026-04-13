using Gym.Application.DTOs.Members;
using Gym.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

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
        
        // Lọc bỏ các lớp PT 1-1 cho trang Marketing (chỉ hiện lớp nhóm)
        if (result.Success && result.Data != null)
        {
            result.Data = result.Data
                .Where(c => c.ClassType != "PT 1-1" && !c.ClassName.Contains("PT 1-1"))
                .ToList();
        }
        
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


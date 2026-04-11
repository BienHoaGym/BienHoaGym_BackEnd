using Gym.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly GymDbContext _context;
    private readonly ILogger<TestController> _logger;

    public TestController(GymDbContext context, ILogger<TestController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("db-connection")]
    public async Task<IActionResult> TestDatabaseConnection()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            if (!canConnect) return StatusCode(500, new { success = false, message = "Cannot connect to database" });
            var memberCount = await _context.Members.CountAsync();
            var packageCount = await _context.MembershipPackages.CountAsync();
            return Ok(new { success = true, stats = new { members = memberCount, packages = packageCount } });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { success = true, status = "Healthy", timestamp = DateTime.UtcNow });
    }

    [HttpGet("sample-packages")]
    public async Task<IActionResult> GetSamplePackages()
    {
        try
        {
            var packages = await _context.MembershipPackages
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.DurationInDays,
                    p.Price,
                    p.DiscountPrice,
                    p.SessionLimit
                })
                .Take(5)
                .ToListAsync();

            return Ok(new { success = true, count = packages.Count, packages });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }
}

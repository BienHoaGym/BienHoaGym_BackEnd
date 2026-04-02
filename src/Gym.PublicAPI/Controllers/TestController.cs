using Gym.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.PublicAPI.Controllers;

/// <summary>
/// Test Controller - Ki?m tra database connection và h? th?ng
/// </summary>
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

    /// <summary>
    /// Test database connection
    /// </summary>
    [HttpGet("db-connection")]
    public async Task<IActionResult> TestDatabaseConnection()
    {
        try
        {
            _logger.LogInformation("Testing database connection...");

            // Test connection
            var canConnect = await _context.Database.CanConnectAsync();

            if (!canConnect)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Cannot connect to database",
                    timestamp = DateTime.UtcNow
                });
            }

            // Get some stats
            var memberCount = await _context.Members.CountAsync();
            var packageCount = await _context.MembershipPackages.CountAsync();
            var roleCount = await _context.Roles.CountAsync();
            var userCount = await _context.Users.CountAsync();

            _logger.LogInformation("Database connection successful!");

            return Ok(new
            {
                success = true,
                message = "Database connection successful",
                database = _context.Database.GetDbConnection().Database,
                stats = new
                {
                    members = memberCount,
                    packages = packageCount,
                    roles = roleCount,
                    users = userCount
                },
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection test failed");

            return StatusCode(500, new
            {
                success = false,
                message = "Database connection failed",
                error = ex.Message,
                innerError = ex.InnerException?.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Get database info
    /// </summary>
    [HttpGet("db-info")]
    public async Task<IActionResult> GetDatabaseInfo()
    {
        try
        {
            var tables = new List<object>();

            // Get table counts
            tables.Add(new { table = "Users", count = await _context.Users.CountAsync() });
            tables.Add(new { table = "Roles", count = await _context.Roles.CountAsync() });
            tables.Add(new { table = "Members", count = await _context.Members.CountAsync() });
            tables.Add(new { table = "MembershipPackages", count = await _context.MembershipPackages.CountAsync() });
            tables.Add(new { table = "MemberSubscriptions", count = await _context.MemberSubscriptions.CountAsync() });
            tables.Add(new { table = "Payments", count = await _context.Payments.CountAsync() });
            tables.Add(new { table = "CheckIns", count = await _context.CheckIns.CountAsync() });
            tables.Add(new { table = "Trainers", count = await _context.Trainers.CountAsync() });
            tables.Add(new { table = "Classes", count = await _context.Classes.CountAsync() });
            tables.Add(new { table = "ClassEnrollments", count = await _context.ClassEnrollments.CountAsync() });

            return Ok(new
            {
                success = true,
                database = _context.Database.GetDbConnection().Database,
                server = _context.Database.GetDbConnection().DataSource,
                tables,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Test API health
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            success = true,
            service = "Gym Management API",
            status = "Healthy",
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Get sample packages
    /// </summary>
    [HttpGet("sample-packages")]
    public async Task<IActionResult> GetSamplePackages()
    {
        try
        {
            // S?a l?i ? dây: S? d?ng dúng tên thu?c tính m?i trong Entity MembershipPackage
            var packages = await _context.MembershipPackages
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Id,
                    p.Name,              // Ðã s?a t? PackageName -> Name
                    p.Description,
                    p.DurationInDays,    // Ðã s?a t? DurationDays -> DurationInDays
                    p.Price,
                    p.DiscountPrice,
                    p.SessionLimit
                })
                .Take(5)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                count = packages.Count,
                packages,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}

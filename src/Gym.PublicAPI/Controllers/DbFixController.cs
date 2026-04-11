using Gym.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DbFixController : ControllerBase
{
    private readonly GymDbContext _context;

    public DbFixController(GymDbContext context)
    {
        _context = context;
    }

    [HttpGet("fix-members")]
    public async Task<IActionResult> FixMembersTable()
    {
        try
        {
            var columns = new[] 
            { 
                "Address", "Gender", "Note", "EmergencyContact", "EmergencyPhone", "FaceEncoding", "QRCode" 
            };

            foreach (var col in columns)
            {
                try
                {
                    // Chạy SQL thô để thêm cột nếu chưa tồn tại
                    #pragma warning disable EF1002
                    await _context.Database.ExecuteSqlRawAsync($"ALTER TABLE Members ADD COLUMN {col} TEXT;");
                    #pragma warning restore EF1002
                }
                catch
                {
                    // Nếu cột đã tồn tại, SQLite sẽ ném lỗi - chúng ta bỏ qua
                }
            }

            return Ok(new { success = true, message = "--- HOÀN TẤT VÁ DATABASE CHO HỘI VIÊN ---" });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }
}

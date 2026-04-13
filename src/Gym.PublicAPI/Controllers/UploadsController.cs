using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // Cho phép upload từ cả Public và Management
public class UploadsController : ControllerBase
{
    private readonly string _uploadsPath;

    public UploadsController(IWebHostEnvironment env)
    {
        // Sử dụng App Root để lưu trữ
        _uploadsPath = Path.Combine(env.ContentRootPath, "uploads");
        if (!Directory.Exists(_uploadsPath)) Directory.CreateDirectory(_uploadsPath);
        
        var profilePath = Path.Combine(_uploadsPath, "profiles");
        if (!Directory.Exists(profilePath)) Directory.CreateDirectory(profilePath);
    }

    [HttpPost("profile-photo")]
    public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Không có file nào được tải lên");

        var extension = Path.GetExtension(file.FileName).ToLower();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        if (!Array.Exists(allowedExtensions, e => e == extension))
            return BadRequest("Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .webp)");

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_uploadsPath, "profiles", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"/uploads/profiles/{fileName}";
        return Ok(new { url = fileUrl });
    }
}

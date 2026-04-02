using Gym.Application.DTOs.Packages;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackagesController> _logger;

    public PackagesController(IPackageService packageService, ILogger<PackagesController> logger)
    {
        _packageService = packageService;
        _logger = logger;
    }

    /// <summary>
    /// Get all packages
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPackages()
    {
        _logger.LogInformation("Getting all packages");

        var result = await _packageService.GetAllAsync();

        return Ok(result);
    }

    /// <summary>
    /// Get package by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPackage(Guid id)
    {
        _logger.LogInformation("Getting package by ID: {PackageId}", id);

        var result = await _packageService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get only active packages
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActivePackages()
    {
        _logger.LogInformation("Getting active packages");

        var result = await _packageService.GetActivePackagesAsync();

        return Ok(result);
    }

    /// <summary>
    /// Create new package
    /// </summary>
    [HttpPost]
    [Authorize(Policy = PermissionConstants.PackageCreate)]
    public async Task<IActionResult> CreatePackage([FromBody] CreatePackageDto dto)
    {
        _logger.LogInformation("Creating new package: {PackageName}", dto.Name);

        var result = await _packageService.CreateAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetPackage), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Update package
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = PermissionConstants.PackageUpdate)]
    public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] UpdatePackageDto dto)
    {
        _logger.LogInformation("Updating package: {PackageId}", id);

        var result = await _packageService.UpdateAsync(id, dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete package (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionConstants.PackageDelete)]
    public async Task<IActionResult> DeletePackage(Guid id)
    {
        _logger.LogInformation("Deleting package: {PackageId}", id);

        var result = await _packageService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

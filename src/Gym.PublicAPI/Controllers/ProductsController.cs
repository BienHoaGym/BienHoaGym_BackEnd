using Gym.Application.DTOs.Billing;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gym.PublicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? type, [FromQuery] string? category)
    {
        var result = await _productService.GetAllAsync(type, category);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionConstants.ProductCreate)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _productService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PermissionConstants.ProductUpdate)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductDto dto)
    {
        var result = await _productService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionConstants.ProductDelete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id}/toggle-status")]
    [Authorize(Policy = PermissionConstants.ProductUpdate)]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _productService.ToggleStatusAsync(id);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _productService.GetCategoriesAsync();
        return Ok(result);
    }
}

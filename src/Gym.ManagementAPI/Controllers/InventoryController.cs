using Gym.Application.DTOs.Inventory;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventories()
    {
        var result = await _inventoryService.GetInventoriesAsync();
        return Ok(result);
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] Guid? productId)
    {
        var result = await _inventoryService.GetStockTransactionsAsync(productId);
        return Ok(result);
    }

    [HttpPost("import")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ImportStock([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.ImportStockAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("export")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ExportStock([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.ExportStockAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("adjustment")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> StockAdjustment([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.StockAdjustmentAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("alerts")]
    public async Task<IActionResult> GetStockAlerts()
    {
        var result = await _inventoryService.GetStockAlertsAsync();
        return Ok(result);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var order = new Order { MemberId = dto.MemberId };
        foreach (var d in dto.Details) 
            order.OrderDetails.Add(new OrderDetail { ProductId = d.ProductId, Quantity = d.Quantity });
            
        var result = await _inventoryService.CreateOrderAsync(order);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

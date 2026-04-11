using Gym.Application.DTOs.Inventory;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Constants;
using Gym.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    [HttpGet("warehouses")]
    public async Task<IActionResult> GetWarehouses()
    {
        var result = await _inventoryService.GetWarehousesAsync();
        return Ok(result);
    }

    [HttpPost("warehouses")]
    [Authorize(Policy = PermissionConstants.InventoryCreate)]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto)
    {
        var result = await _inventoryService.CreateWarehouseAsync(dto);
        return Ok(result);
    }

    [HttpPost("internal-supply")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> CreateInternalSupply([FromBody] CreateInternalSupplyDto dto)
    {
        var result = await _inventoryService.CreateInternalSupplyAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = PermissionConstants.InventoryRead)]
    public async Task<IActionResult> GetInventories([FromQuery] Guid? warehouseId, [FromQuery] bool includeAssets = false)
    {
        var result = await _inventoryService.GetInventoriesAsync(warehouseId, includeAssets);
        return Ok(result);
    }

    [HttpGet("transactions")]
    [Authorize(Policy = PermissionConstants.InventoryRead)]
    public async Task<IActionResult> GetTransactions([FromQuery] Guid? productId, [FromQuery] Guid? warehouseId)
    {
        var result = await _inventoryService.GetStockTransactionsAsync(productId, warehouseId);
        return Ok(result);
    }

    [HttpPost("import")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> ImportStock([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.ImportStockAsync(dto);
        return Ok(result);
    }

    [HttpPost("export")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> ExportStock([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.ExportStockAsync(dto);
        return Ok(result);
    }

    [HttpPost("transfer")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> TransferStock([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.TransferStockAsync(dto);
        return Ok(result);
    }

    [HttpPost("internal-use")]
    [Authorize(Policy = PermissionConstants.InventoryConsume)]
    public async Task<IActionResult> InternalUseStock([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.InternalUseStockAsync(dto);
        return Ok(result);
    }

    [HttpPost("adjustment")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> StockAdjustment([FromBody] CreateStockTransactionDto dto)
    {
        var result = await _inventoryService.StockAdjustmentAsync(dto);
        return Ok(result);
    }

    [HttpGet("alerts")]
    [Authorize(Policy = PermissionConstants.InventoryRead)]
    public async Task<IActionResult> GetStockAlerts()
    {
        var result = await _inventoryService.GetStockAlertsAsync();
        return Ok(result);
    }

    [HttpPost("orders")]
    [Authorize(Policy = PermissionConstants.BillingCreate)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto, [FromQuery] Guid warehouseId)
    {
        var order = new Order { MemberId = dto.MemberId };
        foreach (var d in dto.Details) 
            order.OrderDetails.Add(new OrderDetail { ProductId = d.ProductId, Quantity = d.Quantity });
            
        var result = await _inventoryService.CreateOrderAsync(order, warehouseId);
        return Ok(result);
    }

    // 🕵️ Stock Audit (Kiểm kê)
    [HttpGet("audits")]
    [Authorize(Policy = PermissionConstants.InventoryRead)]
    public async Task<IActionResult> GetStockAudits()
    {
        var result = await _inventoryService.GetStockAuditsAsync();
        return Ok(result);
    }

    [HttpPost("audits")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> CreateStockAudit([FromQuery] Guid warehouseId, [FromQuery] string? note)
    {
        var result = await _inventoryService.CreateStockAuditAsync(warehouseId, note);
        return Ok(result);
    }

    [HttpPut("audits/{auditId}/detail")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> UpdateAuditDetail(Guid auditId, [FromQuery] Guid productId, [FromQuery] int actualQuantity, [FromQuery] string? reason)
    {
        var result = await _inventoryService.UpdateAuditDetailAsync(auditId, productId, actualQuantity, reason);
        return Ok(result);
    }

    [HttpPost("audits/{auditId}/approve")]
    [Authorize(Policy = PermissionConstants.InventoryUpdate)]
    public async Task<IActionResult> ApproveStockAudit(Guid auditId)
    {
        var result = await _inventoryService.ApproveStockAuditAsync(auditId);
        return Ok(result);
    }

    // 📈 Reports
    [HttpGet("turnover")]
    [Authorize(Policy = PermissionConstants.InventoryRead)]
    public async Task<IActionResult> GetStockTurnover()
    {
        var result = await _inventoryService.GetStockTurnoverReportAsync();
        return Ok(result);
    }
}

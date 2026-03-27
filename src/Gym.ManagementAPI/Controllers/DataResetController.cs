using Gym.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DataResetController : ControllerBase
{
    private readonly GymDbContext _context;

    public DataResetController(GymDbContext context)
    {
        _context = context;
    }

    [HttpPost("reset-gym-data")]
    public async Task<IActionResult> ResetData()
    {
        try
        {
            // Danh sách các bảng cần xóa (Sắp xếp theo thứ tự tránh lỗi FK)
            // LƯU Ý: GIỮ LẠI Users, Roles, UserRoles, Trainers (Theo yêu cầu: trừ data quản lý nhân sự)
            
            var sql = @"
                -- 1. Logs / Transactions / Details (Leaf tables first)
                DELETE FROM StockTransactions;
                DELETE FROM EquipmentTransactions;
                DELETE FROM MaintenanceMaterials;
                DELETE FROM MaintenanceLogs;
                DELETE FROM IncidentLogs;
                DELETE FROM EquipmentProviderHistories;
                DELETE FROM Depreciations;
                DELETE FROM CheckIns;
                DELETE FROM ClassEnrollments;
                DELETE FROM TrainerMemberAssignments;
                DELETE FROM InvoiceDetails;
                DELETE FROM OrderDetails;
                
                -- 2. Transactional Masters
                DELETE FROM MemberSubscriptions;
                DELETE FROM Payments;
                DELETE FROM Invoices;
                DELETE FROM Orders;
                DELETE FROM Inventories;

                -- 3. Product / Asset Masters
                DELETE FROM Equipments;
                DELETE FROM EquipmentCategories;
                DELETE FROM Providers;
                DELETE FROM Warehouses;
                DELETE FROM Products;
                DELETE FROM Classes;
                DELETE FROM MembershipPackages;
                
                -- 4. Core Entities
                DELETE FROM Members;
                
                -- 5. System Logs
                DELETE FROM AuditLogs;
            ";

            await _context.Database.ExecuteSqlRawAsync(sql);
            
            return Ok(new { success = true, message = "Đã reset toàn bộ dữ liệu hệ thống (Trừ quản lý nhân sự)." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = $"Lỗi khi reset: {ex.Message}" });
        }
    }
}

using System;
using System.Collections.Generic; // Thêm
using System.Linq; // Thêm
using System.Text.Json;
using System.Threading.Tasks;
using Gym.Application.DTOs.Common; // Thêm
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore; // Thêm

namespace Gym.Application.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(string userId, string action, string entityName, object? oldValues, object? newValues)
        {
            try 
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = false,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string userName = "Hệ thống";
                string userRole = "N/A";

                // Chỉ truy tìm nếu userId là GUID hợp lệ và không phải System
                if (!string.IsNullOrEmpty(userId) && userId != "System" && Guid.TryParse(userId, out var userGuid))
                {
                    try {
                        var user = await _unitOfWork.Users.GetQueryable()
                            .AsNoTracking() // Dùng AsNoTracking để tránh lỗi conflict transaction
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                            .FirstOrDefaultAsync(u => u.Id == userGuid);
                        
                        if (user != null)
                        {
                            userName = user.Username;
                            userRole = user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "User";
                        }
                    } catch {
                        // Nếu lỗi DB khi tìm user thì dùng ID làm tên
                        userName = $"User ({userId.Substring(0, 5)}...)";
                    }
                }

                var log = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = string.IsNullOrEmpty(userId) ? "System" : userId,
                    UserName = userName, 
                    UserRole = userRole,
                    Action = action,
                    EntityName = entityName,
                    OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues, jsonOptions) : "{}",
                    NewValues = newValues != null ? JsonSerializer.Serialize(newValues, jsonOptions) : "{}",
                    Severity = action.Contains("Deleted") ? Domain.Enums.AuditSeverity.Critical : Domain.Enums.AuditSeverity.Normal,
                    Timestamp = DateTime.UtcNow
                };

                await _unitOfWork.AuditLogs.AddAsync(log);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Tuyệt đối không để lỗi ghi log làm hỏng flow chính của app
                Console.WriteLine($" CRITICAL ERROR: Failed to write Audit Log: {ex.Message}");
            }
        }

        // ========================================================
        // THÊM HÀM NÀY ĐỂ FRONTEND CÓ THỂ LẤY DANH SÁCH LOGS
        // ========================================================
        public async Task<ResponseDto<List<AuditLogDto>>> GetAllAsync(string? userId = null, int? severity = null, DateTime? fromDate = null, DateTime? toDate = null, string? action = null)
        {
            var query = _unitOfWork.AuditLogs.GetQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (severity.HasValue)
            {
                query = query.Where(x => (int)x.Severity == severity.Value);
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(x => x.Action.Contains(action));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.Timestamp >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.Timestamp <= toDate.Value);
            }

            var logs = await query
                .OrderByDescending(x => x.Timestamp)
                .Take(500)
                .ToListAsync();

            var dtos = logs.Select(log => new AuditLogDto
            {
                Id = log.Id,
                UserId = log.UserId,
                UserName = string.IsNullOrEmpty(log.UserName) ? "Hệ thống" : log.UserName,
                UserRole = log.UserRole,
                Action = log.Action,
                EntityName = log.EntityName,
                ResourceId = log.ResourceId,
                ResourceName = log.ResourceName,
                OldValues = log.OldValues,
                NewValues = log.NewValues,
                IPAddress = log.IPAddress,
                UserAgent = log.UserAgent,
                Reason = log.Reason,
                Severity = (int)log.Severity,
                Timestamp = log.Timestamp
            }).ToList();

            return ResponseDto<List<AuditLogDto>>.SuccessResult(dtos);
        }
    }
}

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
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            // Fetch user info to avoid empty logs
            string userName = "System";
            string userRole = "N/A";
            if (!string.IsNullOrEmpty(userId) && userId != "System")
            {
                // Note: Assuming a generic way to get user, or just logging the ID if not found
                // For now, let's try to find them if possible, or just use the ID as name if preferred
                // But generally we want to keep logs rich.
                // In a real app, we'd search: var user = await _unitOfWork.Users.GetByIdAsync(new Guid(userId));
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

        // ========================================================
        // THÊM HÀM NÀY ĐỂ FRONTEND CÓ THỂ LẤY DANH SÁCH LOGS
        // ========================================================
        public async Task<ResponseDto<List<AuditLogDto>>> GetAllAsync(string? userId = null, int? severity = null)
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
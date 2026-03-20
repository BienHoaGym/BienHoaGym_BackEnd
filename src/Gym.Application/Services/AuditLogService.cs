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

            var log = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = string.IsNullOrEmpty(userId) ? "System" : userId,
                Action = action,
                EntityName = entityName,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues, jsonOptions) : "{}",
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues, jsonOptions) : "{}",
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.AuditLogs.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        // ========================================================
        // THÊM HÀM NÀY ĐỂ FRONTEND CÓ THỂ LẤY DANH SÁCH LOGS
        // ========================================================
        public async Task<ResponseDto<List<AuditLogDto>>> GetAllAsync()
        {
            // 1. Lấy danh sách Logs gốc
            var logs = await _unitOfWork.AuditLogs.GetQueryable()
                .OrderByDescending(x => x.Timestamp)
                .Take(200)
                .ToListAsync();

            // 2. Lấy ra danh sách ID của những người có trong Log
            var userIds = logs.Where(l => Guid.TryParse(l.UserId, out _))
                              .Select(l => Guid.Parse(l.UserId))
                              .Distinct()
                              .ToList();

            // 3. Truy vấn bảng Users để lấy Tên tương ứng với các ID đó
            var usersDictionary = await _unitOfWork.Users.GetQueryable()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id.ToString(), u => u.FullName);

            // 4. Map (Dịch) ID thành Tên thật
            var dtos = logs.Select(log => new AuditLogDto
            {
                Id = log.Id,
                UserId = log.UserId,
                UserName = usersDictionary.ContainsKey(log.UserId) ? usersDictionary[log.UserId] : (log.UserId == "System" ? "Hệ thống tự động" : log.UserId),
                Action = log.Action,
                EntityName = log.EntityName,
                OldValues = log.OldValues,
                NewValues = log.NewValues,
                Timestamp = log.Timestamp
            }).ToList();

            return ResponseDto<List<AuditLogDto>>.SuccessResult(dtos);
        }
    }
}
using Gym.Application.DTOs.Common;
using Gym.Domain.Entities;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces.Services
{
    public interface IAuditLogService
    {
        /// <summary>
        /// Ghi lại nhật ký thao tác của người dùng vào hệ thống
        /// </summary>
        /// <param name="userId">ID của người thực hiện thao tác</param>
        /// <param name="action">Hành động (VD: CREATE, UPDATE, DELETE, CANCEL)</param>
        /// <param name="entityName">Tên bảng/thực thể bị tác động (VD: Subscriptions)</param>
        /// <param name="oldValues">Dữ liệu cũ trước khi thay đổi (truyền ẩn danh object)</param>
        /// <param name="newValues">Dữ liệu mới sau khi thay đổi (truyền ẩn danh object)</param>
        Task LogAsync(string userId, string action, string entityName, object? oldValues, object? newValues);
        Task<ResponseDto<List<AuditLogDto>>> GetAllAsync(); // Đổi AuditLog thành AuditLogDto    }
    }
}
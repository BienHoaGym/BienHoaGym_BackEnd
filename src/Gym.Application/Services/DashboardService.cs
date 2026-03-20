using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Dashboard;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore; // Để dùng hàm .CountAsync(), .SumAsync()

namespace Gym.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto<DashboardStatsDto>> GetStatsAsync()
    {
        // 1. Đếm tổng thành viên
        // Lưu ý: Cần truy cập DbSet từ UnitOfWork hoặc Repository. 
        // Giả sử UnitOfWork của bạn public DbContext hoặc Repositories.
        // Ở đây tôi ví dụ dùng _unitOfWork.Members (bạn cần đảm bảo Repository có hàm Count hoặc truy cập được IQueryable)

        // Cách an toàn nhất nếu dùng Generic Repository là viết thêm hàm CountAsync, 
        // hoặc lấy GetAll rồi Count (nhưng sẽ chậm).
        // Tốt nhất là Repository nên expose IQueryable hoặc hàm Count.

        // Ví dụ code giả định bạn có thể truy cập IQueryable:
        var totalMembers = await _unitOfWork.Members.GetQueryable().CountAsync();

        var activeMembers = await _unitOfWork.Members.GetQueryable()
            .CountAsync(m => m.Status == MemberStatus.Active);

        var totalTrainers = await _unitOfWork.Trainers.GetQueryable().CountAsync();

        // Tính doanh thu tháng này (Giả sử bảng Payments)
        var currentMonth = DateTime.UtcNow.Month;
        var currentYear = DateTime.UtcNow.Year;

        var monthlyRevenue = await _unitOfWork.Payments.GetQueryable()
            .Where(p => p.PaymentDate.Month == currentMonth &&
                        p.PaymentDate.Year == currentYear &&
                        p.Status == PaymentStatus.Completed)
            .SumAsync(p => p.Amount);

        var stats = new DashboardStatsDto
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            TotalTrainers = totalTrainers,
            MonthlyRevenue = monthlyRevenue
        };

        return ResponseDto<DashboardStatsDto>.SuccessResult(stats);
    }
}
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Dashboard;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore; // Ð? dùng hàm .CountAsync(), .SumAsync()

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
        // 1. Ð?m t?ng thành viên
        // Luu ý: C?n truy c?p DbSet t? UnitOfWork ho?c Repository. 
        // Gi? s? UnitOfWork c?a b?n public DbContext ho?c Repositories.
        // ? dây tôi ví d? dùng _unitOfWork.Members (b?n c?n d?m b?o Repository có hàm Count ho?c truy c?p du?c IQueryable)

        // Cách an toàn nh?t n?u dùng Generic Repository là vi?t thêm hàm CountAsync, 
        // ho?c l?y GetAll r?i Count (nhung s? ch?m).
        // T?t nh?t là Repository nên expose IQueryable ho?c hàm Count.

        // Ví d? code gi? d?nh b?n có th? truy c?p IQueryable:
        var totalMembers = await _unitOfWork.Members.GetQueryable().CountAsync();

        var activeMembers = await _unitOfWork.Members.GetQueryable()
            .CountAsync(m => m.Status == MemberStatus.Active);

        var totalTrainers = await _unitOfWork.Trainers.GetQueryable().CountAsync();

        // Tính doanh thu tháng này (Gi? s? b?ng Payments)
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

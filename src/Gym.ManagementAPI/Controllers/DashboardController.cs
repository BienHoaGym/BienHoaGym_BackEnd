using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.Domain.Constants;
using Microsoft.EntityFrameworkCore; // Cần thêm để dùng .Include() và .ToListAsync()

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IUnitOfWork unitOfWork, ILogger<DashboardController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet("stats")]
    [Authorize(Policy = PermissionConstants.DashboardRead)]
    public async Task<IActionResult> GetDashboardStats()
    {
        _logger.LogInformation("Getting dashboard stats");

        try
        {
            var today = DateTime.UtcNow.Date; // Dùng UTC để đồng bộ với server
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var last6Months = today.AddMonths(-6);

            // 1. Members Statistics
            // Tối ưu: Đếm trực tiếp trong DB thay vì tải hết về
            var totalActiveMembers = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.Status == MemberStatus.Active && !m.IsDeleted);

            var newMembersThisMonth = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.JoinedDate >= monthStart && !m.IsDeleted);

            var prospectiveMembersCount = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.Status == MemberStatus.Prospective && !m.IsDeleted);

            // 2. Subscriptions Statistics
            var activeSubsQuery = _unitOfWork.Subscriptions.GetQueryable()
                .Where(s => s.Status == SubscriptionStatus.Active && !s.IsDeleted);

            var activeSubsCount = await activeSubsQuery.CountAsync();

            var expiryThreshold = today.AddDays(7);
            var expiringSoonCount = await activeSubsQuery
                .CountAsync(s => s.EndDate >= today && s.EndDate <= expiryThreshold);

            var expiredSubsCount = await _unitOfWork.Subscriptions.GetQueryable()
                .CountAsync(s => s.Status == SubscriptionStatus.Expired && !s.IsDeleted);

            // 3. Check-ins Statistics
            var checkinsToday = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime.Date == today && !c.IsDeleted);

            var currentlyInGym = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime.Date == today && c.CheckOutTime == null && !c.IsDeleted);

            // 4. Revenue Statistics (Doanh thu)
            // Lưu ý: Cần Include để truy cập PaymentDate
            var paymentsQuery = _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted);

            var revenueToday = await paymentsQuery
                .Where(p => p.PaymentDate.Date == today)
                .Select(p => (double)p.Amount)
                .SumAsync();

            var revenueMonth = await paymentsQuery
                .Where(p => p.PaymentDate >= monthStart)
                .Select(p => (double)p.Amount)
                .SumAsync();

            var revenueTotal = await paymentsQuery
                .Select(p => (double)p.Amount)
                .SumAsync();

            // 5. Recent Payments (5 giao dịch gần nhất)
            var recentPayments = await paymentsQuery
                .Include(p => p.Subscription).ThenInclude(s => s!.Member) // Nạp Member
                .Include(p => p.Subscription).ThenInclude(s => s!.Package) // Nạp Package
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Amount,
                    Method = p.Method.ToString(),
                    p.PaymentDate,
                    p.TransactionId, // Sửa từ TransactionRef thành TransactionId cho khớp Entity
                    MemberName = p.Subscription != null && p.Subscription.Member != null ? p.Subscription.Member.FullName : "Khách vãng lai",
                    PackageName = p.Subscription != null && p.Subscription.Package != null ? p.Subscription.Package.Name : "N/A"
                })
                .ToListAsync();

            // 6. Expiring Soon List (Danh sách sắp hết hạn chi tiết)
            var expiringSoonList = await activeSubsQuery
                .Where(s => s.EndDate >= today && s.EndDate <= expiryThreshold)
                .Include(s => s.Member) // Quan trọng: Nạp Member để lấy FullName
                .Include(s => s.Package) // Quan trọng: Nạp Package
                .OrderBy(s => s.EndDate)
                .Take(10)
                .Select(s => new
                {
                    s.Id,
                    MemberName = s.Member != null ? s.Member.FullName : "N/A",
                    MemberCode = s.Member != null ? s.Member.MemberCode : "N/A",
                    PhoneNumber = s.Member != null ? s.Member.PhoneNumber : "N/A",
                    PackageName = s.Package != null ? s.Package.Name : "N/A",
                    s.EndDate,
                    DaysLeft = (s.EndDate - today).Days
                })
                .ToListAsync();

            // 7. Revenue Chart Data (Biểu đồ doanh thu 6 tháng)
            // Phần này xử lý trên RAM sau khi lấy dữ liệu thô để tối ưu Query phức tạp
            var rawRevenueData = await paymentsQuery
                .Where(p => p.PaymentDate >= last6Months)
                .Select(p => new { p.PaymentDate, p.Amount })
                .ToListAsync();

            var revenueByMonth = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var m = today.AddMonths(-i);
                    var start = new DateTime(m.Year, m.Month, 1);
                    var end = start.AddMonths(1);

                    var total = rawRevenueData
                        .Where(p => p.PaymentDate >= start && p.PaymentDate < end)
                        .Sum(p => p.Amount);

                    return new { Month = m.ToString("MM/yyyy"), Revenue = total };
                })
                .Reverse()
                .ToList();

            // Tổng hợp kết quả trả về
            var result = new
            {
                // KPI Cards
                TotalActiveMembers = totalActiveMembers,
                NewMembersThisMonth = newMembersThisMonth,
                ActiveSubscriptions = activeSubsCount,
                ExpiringIn7Days = expiringSoonCount,
                ExpiredSubscriptions = expiredSubsCount,
                CheckInsToday = checkinsToday,
                CurrentlyInGym = currentlyInGym,
                ProspectiveMembersCount = prospectiveMembersCount,

                // Revenue
                RevenueToday = revenueToday,
                RevenueThisMonth = revenueMonth,
                RevenueTotal = revenueTotal,

                // Charts & Lists
                RevenueByMonth = revenueByMonth,
                RecentPayments = recentPayments,
                ExpiringSoonList = expiringSoonList,

                GeneratedAt = DateTime.UtcNow
            };

            return Ok(ResponseDto<object>.SuccessResult(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            // Trả về lỗi chi tiết hơn trong môi trường Dev nếu cần
            return StatusCode(500, ResponseDto<object>.FailureResult($"Lỗi hệ thống: {ex.Message}"));
        }
    }

    [HttpGet("checkin-chart")]
    [Authorize(Policy = PermissionConstants.DashboardRead)]
    public async Task<IActionResult> GetCheckinChart()
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var sevenDaysAgo = today.AddDays(-6);

            // Tối ưu: Chỉ lấy dữ liệu trong khoảng 7 ngày qua
            var rawCheckins = await _unitOfWork.CheckIns.GetQueryable()
                .Where(c => c.CheckInTime >= sevenDaysAgo && !c.IsDeleted)
                .Select(c => c.CheckInTime)
                .ToListAsync();

            var chart = Enumerable.Range(0, 7)
                .Select(i =>
                {
                    var date = today.AddDays(-i);
                    var count = rawCheckins.Count(c => c.Date == date);
                    return new { Date = date.ToString("dd/MM"), Count = count };
                })
                .Reverse()
                .ToList();

            return Ok(ResponseDto<object>.SuccessResult(chart));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting checkin chart");
            return StatusCode(500, ResponseDto<object>.FailureResult(ex.Message));
        }
    }
}

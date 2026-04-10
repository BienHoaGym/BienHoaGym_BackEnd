using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gym.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Gym.PublicAPI.Controllers;

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
            var utcNow = DateTime.UtcNow;
            var today = DateTime.SpecifyKind(utcNow.Date, DateTimeKind.Utc);
            var tomorrow = today.AddDays(1);
            var monthStart = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var last6Months = today.AddMonths(-6);
            var yesterday = today.AddDays(-1);
            var lastMonthStart = monthStart.AddMonths(-1);
            var lastMonthEnd = monthStart.AddDays(-1);
            var expiryThreshold = today.AddDays(7);

            // 1. Members Statistics
            var totalActiveMembers = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.Status == MemberStatus.Active && !m.IsDeleted);

            if (totalActiveMembers == 0)
            {
                return Ok(ResponseDto<object>.SuccessResult(GetMockDashboardStats(today, monthStart, last6Months), "Đang ở chế độ Demo (Cơ sở dữ liệu trống)"));
            }

            var activeMembersLastMonth = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.Status == MemberStatus.Active && m.JoinedDate <= lastMonthEnd && !m.IsDeleted);
            var memberGrowth = activeMembersLastMonth == 0 ? 100 : Math.Round(((double)(totalActiveMembers - activeMembersLastMonth) / activeMembersLastMonth) * 100, 1);

            var newMembersThisMonth = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.JoinedDate >= monthStart && !m.IsDeleted);

            var prospectiveMembersCount = await _unitOfWork.Members.GetQueryable()
                .CountAsync(m => m.Status == MemberStatus.Prospective && !m.IsDeleted);

            // 2. Subscriptions
            var activeSubsQuery = _unitOfWork.Subscriptions.GetQueryable()
                .Where(s => s.Status == SubscriptionStatus.Active && !s.IsDeleted);
            var activeSubsCount = await activeSubsQuery.CountAsync();
            var expiringSoonCount = await activeSubsQuery
                .CountAsync(s => s.EndDate >= today && s.EndDate <= expiryThreshold);

            // 3. Check-ins
            var checkinsToday = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime >= today && c.CheckInTime < tomorrow && !c.IsDeleted);
            var checkinsYesterday = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime >= yesterday && c.CheckInTime < today && !c.IsDeleted);
            var checkinTrend = checkinsYesterday == 0 ? (checkinsToday > 0 ? 100 : 0) : Math.Round(((double)(checkinsToday - checkinsYesterday) / checkinsYesterday) * 100, 1);
            var currentlyInGym = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime >= today && c.CheckInTime < tomorrow && c.CheckOutTime == null && !c.IsDeleted);

            // 4. Revenue (Combined Payments & Invoices)
            var paymentsQuery = _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted);
            var invoicesQuery = _unitOfWork.Invoices.GetQueryable()
                .Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted);

            var revenueToday = (double)await paymentsQuery.Where(p => p.PaymentDate >= today && p.PaymentDate < tomorrow).Select(p => (double)p.Amount).SumAsync()
                             + (double)await invoicesQuery.Where(i => i.CreatedAt >= today && i.CreatedAt < tomorrow).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();

            var revenueYesterday = (double)await paymentsQuery.Where(p => p.PaymentDate >= yesterday && p.PaymentDate < today).Select(p => (double)p.Amount).SumAsync()
                                 + (double)await invoicesQuery.Where(i => i.CreatedAt >= yesterday && i.CreatedAt < today).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();

            var revenueTrend = revenueYesterday == 0 ? (revenueToday > 0 ? 100 : 0) : Math.Round(((double)(revenueToday - revenueYesterday) / revenueYesterday) * 100, 1);

            var revenueMonthDirect = (double)await paymentsQuery.Where(p => p.PaymentDate >= monthStart).Select(p => (double)p.Amount).SumAsync()
                                   + (double)await invoicesQuery.Where(i => i.CreatedAt >= monthStart).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();

            var revenueMonthLast = (double)await paymentsQuery.Where(p => p.PaymentDate >= lastMonthStart && p.PaymentDate <= lastMonthEnd).Select(p => (double)p.Amount).SumAsync()
                                 + (double)await invoicesQuery.Where(i => i.CreatedAt >= lastMonthStart && i.CreatedAt <= lastMonthEnd).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();

            var revenueTotal = (double)await paymentsQuery.Select(p => (double)p.Amount).SumAsync()
                             + (double)await invoicesQuery.Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();

            // 5. Advanced Data
            var recentPayments = await paymentsQuery
                .Include(p => p.Subscription).ThenInclude(s => s!.Member)
                .Include(p => p.Subscription).ThenInclude(s => s!.Package)
                .OrderByDescending(p => p.PaymentDate).Take(5)
                .Select(p => new { p.Id, p.Amount, Method = p.Method.ToString(), p.PaymentDate, p.TransactionId, MemberName = p.Subscription != null && p.Subscription.Member != null ? p.Subscription.Member.FullName : "Khách vãng lai", PackageName = p.Subscription != null && p.Subscription.Package != null ? p.Subscription.Package.Name : "N/A" })
                .ToListAsync();

            var revenueByPackageRaw = await _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && p.Subscription != null && p.Subscription.Package != null)
                .GroupBy(p => p.Subscription!.Package!.Name)
                .Select(g => new { Category = g.Key ?? "N/A", Value = g.Sum(p => (double)p.Amount) })
                .ToListAsync();

            var revenueByPackage = revenueByPackageRaw
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            var rawCheckins = await _unitOfWork.CheckIns.GetQueryable()
               .Where(c => c.CheckInTime >= today.AddDays(-6) && !c.IsDeleted)
               .Select(c => c.CheckInTime.Date).ToListAsync();
            var checkinChartData = Enumerable.Range(0, 7)
               .Select(i => {
                   var date = today.AddDays(-6 + i);
                   return new { Date = date.ToString("dd/MM"), Count = rawCheckins.Count(c => c == date) };
               }).ToList();

            var insights = new List<string>();
            if (revenueTrend < 0) insights.Add($"Doanh thu giảm {Math.Abs(revenueTrend)}% so với hôm qua.");
            if (checkinTrend > 10) insights.Add($"Lượng khách đang tăng mạnh (+{checkinTrend}%).");
            if (expiringSoonCount > 0) insights.Add($"Có {expiringSoonCount} hội viên sắp hết hạn.");

            var result = new
            {
                RevenueToday = new { Value = revenueToday, Trend = revenueTrend, Label = "Doanh thu hôm nay" },
                CheckInsToday = new { Value = checkinsToday, Trend = checkinTrend, Label = "Lượt check-in", Detail = "vs Hôm qua" },
                ActiveMembers = new { Value = totalActiveMembers, Trend = memberGrowth, Label = "Hội viên đang tập" },
                ExpiringSubscriptionsCount = new { Value = expiringSoonCount, Label = "Gói sắp hết hạn" },
                Occupancy = new { Current = currentlyInGym, Percentage = (double)currentlyInGym / 100 * 100, PeakHour = "17:00 - 19:00" },
                RevenueMonth = revenueMonthDirect,
                RevenueProgress = Math.Min(100, (revenueMonthDirect / 50000000.0) * 100),
                RevenueByPackage = revenueByPackage,
                RevenueByMonth = Enumerable.Range(0, 6).Select(i => new { Month = today.AddMonths(-i).ToString("MM/yyyy"), Revenue = 0.0 }).ToList(), // Simplified for brevity
                CheckinChartData = checkinChartData,
                Insights = insights,
                GeneratedAt = DateTime.UtcNow
            };

            return Ok(ResponseDto<object>.SuccessResult(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            return StatusCode(500, ResponseDto<object>.FailureResult($"Lỗi hệ thống: {ex.Message} | Detail: {ex.InnerException?.Message}"));
        }
    }

    private object GetMockDashboardStats(DateTime today, DateTime monthStart, DateTime last6Months)
    {
        return new
        {
            IsDemo = true,
            RevenueToday = new { Value = 1250000, Trend = 15.5, Label = "Doanh thu hôm nay (Demo)" },
            CheckInsToday = new { Value = 42, Trend = 8.2, Label = "Lượt khách (Demo)" },
            ActiveMembers = new { Value = 156, Trend = 12.0, Label = "Hội viên (Demo)" },
            ExpiringSubscriptionsCount = new { Value = 8, Label = "Sắp hết hạn" },
            Occupancy = new { Current = 35, Max = 100, Percentage = 35.0, Status = "An toàn", PeakHour = "17:00 - 19:00" },
            RevenueMonth = 45800000,
            RevenueProgress = 91.6,
            RevenueByPackage = new List<object> { new { Category = "Gói Gym", Value = 25000000.0 }, new { Category = "Gói PT", Value = 20800000.0 } },
            RevenueByMonth = Enumerable.Range(0, 6).Select(i => new { Month = today.AddMonths(-i).ToString("MM/yyyy"), Revenue = 35000000.0 }).Reverse().ToList(),
            CheckinChartData = Enumerable.Range(0, 7).Select(i => new { Date = today.AddDays(-6 + i).ToString("dd/MM"), Count = 25 }).ToList(),
            Insights = new List<string> { "Ứng dụng đang hiển thị dữ liệu DEMO." },
            GeneratedAt = DateTime.UtcNow
        };
    }

    [HttpGet("checkin-chart")]
    [Authorize(Policy = PermissionConstants.DashboardRead)]
    public async Task<IActionResult> GetCheckinChart()
    {
        try {
            var utcNow = DateTime.UtcNow;
            var today = DateTime.SpecifyKind(utcNow.Date, DateTimeKind.Utc);
            var sevenDaysAgo = today.AddDays(-6);
            var rawCheckins = await _unitOfWork.CheckIns.GetQueryable().Where(c => c.CheckInTime >= sevenDaysAgo && !c.IsDeleted).Select(c => c.CheckInTime).ToListAsync();
            var chart = Enumerable.Range(0, 7).Select(i => {
                var date = today.AddDays(-i);
                return new { Date = date.ToString("dd/MM"), Count = rawCheckins.Count(c => c.Date == date) };
            }).Reverse().ToList();
            return Ok(ResponseDto<object>.SuccessResult(chart));
        } catch (Exception ex) {
            return StatusCode(500, ResponseDto<object>.FailureResult(ex.Message));
        }
    }
}

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
            var utcNow = DateTime.UtcNow;
            var today = DateTime.SpecifyKind(utcNow.Date, DateTimeKind.Utc);
            var tomorrow = today.AddDays(1);
            var monthStart = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var last6Months = today.AddMonths(-6);
            var yesterday = today.AddDays(-1);
            var lastMonthStart = monthStart.AddMonths(-1);
            var lastMonthEnd = monthStart.AddDays(-1);
            var expiryThreshold = today.AddDays(7);

            // 1. Members Statistics & Insights
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

            // 2. Subscriptions Statistics
            var activeSubsQuery = _unitOfWork.Subscriptions.GetQueryable()
                .Where(s => s.Status == SubscriptionStatus.Active && !s.IsDeleted);

            var activeSubsCount = await activeSubsQuery.CountAsync();

            var expiringSoonCount = await _unitOfWork.Subscriptions.GetQueryable()
                .Where(s => !s.IsDeleted && (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Pending))
                .GroupBy(s => s.MemberId)
                .Select(g => g.Max(s => s.EndDate))
                .CountAsync(endDate => endDate >= today && endDate <= expiryThreshold);

            var expiredSubsCount = await _unitOfWork.Subscriptions.GetQueryable()
                .CountAsync(s => s.Status == SubscriptionStatus.Expired && !s.IsDeleted);

            // 3. Check-ins Statistics & Insights
            var checkinsToday = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime >= today && c.CheckInTime < tomorrow && !c.IsDeleted);

            var checkinsYesterday = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime >= yesterday && c.CheckInTime < today && !c.IsDeleted);

            var checkinTrend = checkinsYesterday == 0 ? (checkinsToday > 0 ? 100 : 0) : Math.Round(((double)(checkinsToday - checkinsYesterday) / checkinsYesterday) * 100, 1);

            var currentlyInGym = await _unitOfWork.CheckIns.GetQueryable()
                .CountAsync(c => c.CheckInTime >= today && c.CheckInTime < tomorrow && c.CheckOutTime == null && !c.IsDeleted);

            // 4. Revenue Statistics & Insights
            var paymentsQuery = _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted);

            var paymentsToday = await paymentsQuery
                .Where(p => p.PaymentDate >= today && p.PaymentDate < tomorrow)
                .ToListAsync();
            var revenueToday = (double)paymentsToday.Sum(p => p.Amount);

            var paymentsYesterday = await paymentsQuery
                .Where(p => p.PaymentDate >= yesterday && p.PaymentDate < today)
                .ToListAsync();
            var revenueYesterday = (double)paymentsYesterday.Sum(p => p.Amount);

            var revenueTrend = revenueYesterday == 0 ? (revenueToday > 0 ? 100 : 0) : Math.Round(((double)(revenueToday - revenueYesterday) / revenueYesterday) * 100, 1);

            var paymentsMonth = await paymentsQuery
                .Where(p => p.PaymentDate >= monthStart)
                .ToListAsync();
            var revenueMonthDirect = (double)paymentsMonth.Sum(p => p.Amount);

            var paymentsLastMonth = await paymentsQuery
                .Where(p => p.PaymentDate >= lastMonthStart && p.PaymentDate <= lastMonthEnd)
                .ToListAsync();
            var revenueMonthLast = (double)paymentsLastMonth.Sum(p => p.Amount);

            var revenueTotal = await paymentsQuery
                .Select(p => (double)p.Amount)
                .SumAsync();

            // 5. Recent Payments
            var recentPayments = await paymentsQuery
                .Include(p => p.Subscription).ThenInclude(s => s!.Member)
                .Include(p => p.Subscription).ThenInclude(s => s!.Package)
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Amount,
                    Method = p.Method.ToString(),
                    p.PaymentDate,
                    p.TransactionId,
                    MemberName = p.Subscription != null && p.Subscription.Member != null ? p.Subscription.Member.FullName : "Khách vãng lai",
                    PackageName = p.Subscription != null && p.Subscription.Package != null ? p.Subscription.Package.Name : "N/A"
                })
                .ToListAsync();

            // 6. Expiring Soon List (Added PhoneNumber for actions)
            var expiringSoonList = await _unitOfWork.Subscriptions.GetQueryable()
                .Where(s => !s.IsDeleted && (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Pending))
                .Where(s => s.EndDate >= today && s.EndDate <= expiryThreshold)
                // CHẶN: Chỉ lấy những gói là gói MỚI NHẤT của hội viên (nếu đã gia hạn thì không hiện gói cũ sắp hết hạn nữa)
                .Where(s => !_unitOfWork.Subscriptions.GetQueryable()
                    .Any(futureSub => futureSub.MemberId == s.MemberId && 
                                     !futureSub.IsDeleted && 
                                     (futureSub.Status == SubscriptionStatus.Active || futureSub.Status == SubscriptionStatus.Pending) && 
                                     futureSub.EndDate > s.EndDate))
                .Include(s => s.Member)
                .Include(s => s.Package)
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
                    DaysLeft = (s.EndDate - today).Days,
                    Status = (s.EndDate - today).Days <= 3 ? "Urgent" : "Warning"
                })
                .ToListAsync();

            // 7. Revenue Chart Data
            var rawRevenueData = await paymentsQuery
                .Where(p => p.PaymentDate >= last6Months)
                .Select(p => new { p.PaymentDate, p.Amount })
                .ToListAsync();

            var revenueByMonth = Enumerable.Range(0, 6)
                .Select(i =>
                {
                    var m = today.AddMonths(-i);
                    var start = new DateTime(m.Year, m.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    var end = start.AddMonths(1);
                    var total = rawRevenueData.Where(p => p.PaymentDate >= start && p.PaymentDate < end).Sum(p => p.Amount);
                    return new { Month = m.ToString("MM/yyyy"), Revenue = total };
                })
                .Reverse()
                .ToList();

            // 8. Operational Metrics & Alerts
            var activeTrainersCount = await _unitOfWork.Trainers.GetQueryable()
                .CountAsync(t => t.IsActive && !t.IsDeleted);

            var equipmentBrokenCount = await _unitOfWork.Equipments.GetQueryable()
                .CountAsync(e => e.Status == EquipmentStatus.Broken && !e.IsDeleted);

            var maintenanceRequiredCount = await _unitOfWork.Equipments.GetQueryable()
                .CountAsync(e => (e.Status == EquipmentStatus.Maintenance || (e.NextMaintenanceDate != null && e.NextMaintenanceDate <= today)) && !e.IsDeleted);

            // Thống kê Doanh thu theo gói
            var revenueByPackage = await _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && p.Subscription != null && p.Subscription.Package != null)
                .GroupBy(p => p.Subscription!.Package!.Name)
                .Select(g => new { Category = g.Key ?? "N/A", Value = g.Sum(p => (double)p.Amount) })
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToListAsync();

            // --- NEW: REVENUE TARGET LOGIC (2.3) ---
            decimal monthlyTarget = 50000000; // Mock target 50M
            var targetProgress = monthlyTarget > 0 ? (double)(revenueMonthDirect / (double)monthlyTarget) * 100 : 0;

            // --- NEW: OCCUPANCY LOGIC (2.2) ---
            int maxCapacity = 100;
            double occupancyPercent = (double)currentlyInGym / maxCapacity * 100;
            string occupancyStatus = occupancyPercent > 80 ? "Quá tải" : (occupancyPercent > 50 ? "Bình thường" : "An toàn");
            
            // Mock peak hour (In real app, analyze hourly checkins)
            string peakHourToday = "17:00 - 19:00"; 

            // --- NEW: GLOBAL INSIGHTS (2.6) ---
            var insights = new List<string>();
            if (revenueTrend < 0) insights.Add($"Doanh thu giảm {Math.Abs(revenueTrend)}% so với hôm qua.");
            if (checkinTrend > 10) insights.Add($"Lượng khách đang tăng mạnh (+{checkinTrend}%).");
            if (occupancyPercent < 10) insights.Add("Phòng tập đang vắng khách, thời điểm tốt để vệ sinh thiết bị.");
            if (expiringSoonCount > 0) insights.Add($"Có {expiringSoonCount} hội viên sắp hết hạn, cần gia hạn ngay.");
            if (equipmentBrokenCount > 0) insights.Add($"Cảnh báo: {equipmentBrokenCount} thiết bị đang hỏng cần sửa chữa.");
            if (targetProgress < 50 && today.Day > 15) insights.Add("Tiến độ doanh thu tháng đang chậm hơn so với mục tiêu.");

            // --- NEW: ADVANCED ANALYTICS (2.2) ---
            var sevenDaysAgo = today.AddDays(-6);
            var rawCheckins = await _unitOfWork.CheckIns.GetQueryable()
               .Where(c => c.CheckInTime >= sevenDaysAgo && !c.IsDeleted)
               .Select(c => c.CheckInTime.Date)
               .ToListAsync();

            var checkinChartData = Enumerable.Range(0, 7)
               .Select(i =>
               {
                   var date = sevenDaysAgo.AddDays(i);
                   var count = rawCheckins.Count(c => c == date);
                   return new { Date = date.ToString("dd/MM"), Count = count };
               })
               .ToList();

            // --- NEW: MEMBERSHIP QUICK VIEW (2.3) ---
            var newMembersList = await _unitOfWork.Members.GetQueryable()
                .Where(m => m.JoinedDate >= today.AddDays(-7) && !m.IsDeleted)
                .OrderByDescending(m => m.JoinedDate)
                .Take(5)
                .Select(m => new { m.Id, m.FullName, m.MemberCode, m.JoinedDate, m.PhoneNumber })
                .ToListAsync();

            // --- NEW: TODAY'S SCHEDULE (2.5) ---
            var dayOfWeek = today.DayOfWeek.ToString(); // Monday, Tuesday...
            var currentTime = DateTime.UtcNow.TimeOfDay;

            var classesTodayRaw = await _unitOfWork.Classes.GetQueryable()
                .Where(c => c.ScheduleDay == dayOfWeek && c.IsActive && !c.IsDeleted)
                .Include(c => c.Trainer)
                .ToListAsync();
            
            // Sort in memory because SQLite/EF doesn't like TimeSpan OrderBy
            classesTodayRaw = classesTodayRaw.OrderBy(c => c.StartTime).ToList();
            
            var classesToday = new List<object>();
            foreach (var c in classesTodayRaw)
            {
                classesToday.Add(new {
                    c.Id,
                    c.ClassName,
                    Time = c.StartTime.ToString(@"hh\:mm"),
                    TrainerName = c.Trainer != null ? c.Trainer.FullName : "TBA",
                    Status = c.StartTime > currentTime ? "Sắp diễn ra" : (c.EndTime < currentTime ? "Đã xong" : "Đang học"),
                    Capacity = c.MaxCapacity,
                    Enrolled = c.CurrentEnrollment
                });
            }

            var result = new
            {
                // KPI Cards with Trends (2.1)
                RevenueToday = new { Value = revenueToday, Trend = revenueTrend, Label = "Doanh thu hôm nay", Status = revenueTrend >= 0 ? "Tốt" : "Cần chú ý" },
                CheckInsToday = new { Value = checkinsToday, Trend = checkinTrend, Label = "Lượt check-in", Detail = checkinsYesterday == 0 ? "Khởi đầu mới" : (checkinTrend > 0 ? $"Tăng {checkinTrend}%" : $"Giảm {Math.Abs(checkinTrend)}%") + " so với hôm qua" },
                ActiveMembers = new { Value = totalActiveMembers, Trend = memberGrowth, Label = "Hội viên đang tập", Detail = $"{memberGrowth}% so với tháng trước" },
                ExpiringSubscriptionsCount = new { Value = expiringSoonCount, Label = "Gói sắp hết hạn", NeedsAction = expiringSoonCount > 0 },
                ActiveTrainersCount = new { Value = activeTrainersCount, Label = "PT đang dạy", Status = "Normal" },
                EquipmentAlertCount = new { Value = equipmentBrokenCount, Label = "Thiết bị lỗi", Status = equipmentBrokenCount > 0 ? "Urgent" : "Good" },

                // Operational Insights (2.2 & 2.5)
                Occupancy = new { 
                    Current = currentlyInGym, 
                    Max = maxCapacity, 
                    Percentage = occupancyPercent, 
                    Status = occupancyStatus,
                    PeakHour = peakHourToday,
                    Alert = occupancyPercent > 90
                },
                EquipmentHealth = new { 
                    Total = await _unitOfWork.Equipments.GetQueryable().CountAsync(e => !e.IsDeleted), 
                    Broken = equipmentBrokenCount, 
                    Maintenance = maintenanceRequiredCount,
                    Status = equipmentBrokenCount > 0 ? "Danger" : (maintenanceRequiredCount > 0 ? "Warning" : "Success")
                },
                ActiveTrainers = activeTrainersCount,
                ProspectiveLeads = prospectiveMembersCount,

                // Financial Insights (2.3)
                RevenueMonth = revenueMonthDirect,
                RevenueTarget = monthlyTarget,
                RevenueProgress = Math.Min(100, Math.Round(targetProgress, 1)),
                RevenueGrowthMonth = revenueMonthLast == 0 ? 100 : Math.Round(((double)(revenueMonthDirect - revenueMonthLast) / revenueMonthLast) * 100, 1),
                RevenueTotal = revenueTotal,
                RevenueByPackage = revenueByPackage,

                // Charts (2.2)
                RevenueByMonth = revenueByMonth,
                CheckinChartData = checkinChartData,

                // Global AI-lite Insights (2.6)
                Insights = insights,

                // Lists & Schedules (2.4 & 2.5)
                ExpiringSoonList = expiringSoonList,
                RecentPayments = recentPayments,
                NewMembersList = newMembersList,
                ClassesToday = classesToday,

                GeneratedAt = DateTime.UtcNow
            };

            return Ok(ResponseDto<object>.SuccessResult(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            // Trả về lỗi chi tiết để debug trên host (Tạm thời)
            return StatusCode(500, ResponseDto<object>.FailureResult($"Lỗi hệ thống: {ex.Message} | Detail: {ex.InnerException?.Message}"));
        }
    }

    [HttpGet("checkin-chart")]
    [Authorize(Policy = PermissionConstants.DashboardRead)]
    public async Task<IActionResult> GetCheckinChart()
    {
        try
        {
            var utcNow = DateTime.UtcNow;
            var today = DateTime.SpecifyKind(utcNow.Date, DateTimeKind.Utc);
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

    private object GetMockDashboardStats(DateTime today, DateTime monthStart, DateTime last6Months)
    {
        return new
        {
            IsDemo = true,
            RevenueToday = new { Value = 1250000, Trend = 15.5, Label = "Doanh thu hôm nay (Demo)", Status = "Tốt" },
            CheckInsToday = new { Value = 42, Trend = 8.2, Label = "Lượt khách (Demo)", Detail = "Tăng 8% so với hôm qua" },
            ActiveMembers = new { Value = 156, Trend = 12.0, Label = "Hội viên (Demo)", Detail = "12% trong tháng này" },
            ExpiringSubscriptionsCount = new { Value = 8, Label = "Sắp hết hạn", NeedsAction = true },
            ActiveTrainersCount = new { Value = 5, Label = "PT Đang dạy", Status = "Normal" },
            EquipmentAlertCount = new { Value = 2, Label = "Thiết bị lỗi", Status = "Urgent" },

            Occupancy = new { Current = 35, Max = 100, Percentage = 35.0, Status = "An toàn", PeakHour = "17:00 - 19:00", Alert = false },
            EquipmentHealth = new { Total = 45, Broken = 2, Maintenance = 3, Status = "Warning" },
            ActiveTrainers = 5,
            ProspectiveLeads = 12,

            RevenueMonth = 45800000,
            RevenueTarget = 50000000,
            RevenueProgress = 91.6,
            RevenueGrowthMonth = 15.4,
            RevenueTotal = 1250000000,
            RevenueByPackage = new List<object> {
                new { Category = "Gói Gym 1 Tháng", Value = 15000000.0 },
                new { Category = "Gói Gym 1 Năm", Value = 25000000.0 },
                new { Category = "Gói PT 1:1", Value = 35000000.0 },
                new { Category = "Hội viên VIP", Value = 20000000.0 }
            },

            RevenueByMonth = Enumerable.Range(0, 6).Select(i => {
                var m = today.AddMonths(-i);
                return new { Month = m.ToString("MM/yyyy"), Revenue = 35000000 + (new Random().Next(-5000000, 5000000)) };
            }).Reverse().ToList(),

            CheckinChartData = Enumerable.Range(0, 7).Select(i => {
                var date = today.AddDays(-6 + i);
                return new { Date = date.ToString("dd/MM"), Count = 20 + (new Random().Next(0, 30)) };
            }).ToList(),

            Insights = new List<string> {
                "Hệ thống đang chạy ở chế độ DEMO do cơ sở dữ liệu trống.",
                "Hội viên 'Nguyễn Văn Demo' sắp hết hạn gói tập.",
                "Doanh thu tháng này sắp đạt mục tiêu đề ra (91%).",
                "Phòng tập đang ở trạng thái tải trọng an toàn (35%)."
            },

            ExpiringSoonList = new List<object> {
                new { Id = Guid.NewGuid(), MemberName = "Nguyễn Văn Demo", MemberCode = "M1001", PhoneNumber = "0900000001", PackageName = "Gói Gym 12 Tháng", EndDate = today.AddDays(2), DaysLeft = 2, Status = "Urgent" },
                new { Id = Guid.NewGuid(), MemberName = "Trần Thị Demo", MemberCode = "M1002", PhoneNumber = "0900000002", PackageName = "Gói PT 10 Buổi", EndDate = today.AddDays(5), DaysLeft = 5, Status = "Warning" }
            },
            RecentPayments = new List<object> {
                new { Id = Guid.NewGuid(), Amount = 500000, Method = "Cash", PaymentDate = DateTime.UtcNow.AddMinutes(-45), TransactionId = "TX123", MemberName = "Lê Văn Demo", PackageName = "Gói Tháng" },
                new { Id = Guid.NewGuid(), Amount = 4500000, Method = "BankTransfer", PaymentDate = DateTime.UtcNow.AddHours(-3), TransactionId = "TX124", MemberName = "Phạm Thị Demo", PackageName = "Gói Năm" }
            },
            NewMembersList = new List<object> {
                new { Id = Guid.NewGuid(), FullName = "Hoàng Demo", MemberCode = "M2001", JoinedDate = today.AddDays(-1), PhoneNumber = "0912345678" },
                new { Id = Guid.NewGuid(), FullName = "Đỗ Demo", MemberCode = "M2002", JoinedDate = today.AddDays(-3), PhoneNumber = "0987654321" }
            },
            ClassesToday = new List<object> {
                new { Id = Guid.NewGuid(), ClassName = "Yoga Sáng", Time = "08:00", TrainerName = "HLV Minh", Status = "Đã xong", Capacity = 20, Enrolled = 15 },
                new { Id = Guid.NewGuid(), ClassName = "Zumba Dance", Time = "17:30", TrainerName = "HLV Lan", Status = "Sắp diễn ra", Capacity = 25, Enrolled = 18 }
            },

            GeneratedAt = DateTime.UtcNow
        };
    }
}


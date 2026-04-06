using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Reports;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Services;

public class ReportsService : IReportsService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto<RevenueReportDto>> GetRevenueReportAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var today = DateTime.UtcNow.Date;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var yearStart = new DateTime(today.Year, 1, 1);

        // Filters (Default last 30 days)
        var start = startDate ?? today.AddDays(-29);
        var end = endDate ?? today.AddDays(1).AddSeconds(-1);

        // --- REVENUE DATA SOURCES ---
        var paymentsQuery = _unitOfWork.Payments.GetQueryable()
            .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted)
            .Include(p => p.Subscription).ThenInclude(s => s!.Package);

        var invoicesQuery = _unitOfWork.Invoices.GetQueryable()
            .Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted)
            .Include(i => i.Details);

        // 1. Overview (Monthly/Yearly/Today)
        var revToday = await paymentsQuery.Where(p => p.PaymentDate.Date == today).Select(p => (double)p.Amount).SumAsync() +
                       await invoicesQuery.Where(i => i.CreatedAt.Date == today).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();
                       
        var revMonth = await paymentsQuery.Where(p => p.PaymentDate >= monthStart).Select(p => (double)p.Amount).SumAsync() +
                       await invoicesQuery.Where(i => i.CreatedAt >= monthStart).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();
                       
        var revYear = await paymentsQuery.Where(p => p.PaymentDate >= yearStart).Select(p => (double)p.Amount).SumAsync() +
                      await invoicesQuery.Where(i => i.CreatedAt >= yearStart).Select(i => (double)(i.TotalAmount - i.DiscountAmount)).SumAsync();

        // Expenses for this month (Module Kho & Thiết bị)
        var matExpense = await _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .Where(t => t.Date >= monthStart && 
                       (t.Type == StockTransactionType.InternalUse || 
                        t.Type == StockTransactionType.Damage ||
                        t.Type == StockTransactionType.Loss))
            .Select(t => (double)(t.Quantity * (t.Product != null ? t.Product.CostPrice : 0)))
            .SumAsync();
        
        var maintExpense = await _unitOfWork.MaintenanceLogs.GetQueryable()
            .Where(m => m.Date >= monthStart && m.Status == MaintenanceStatus.Completed)
            .Select(m => (double)m.Cost)
            .SumAsync();
            
        var depExpense = await _unitOfWork.Depreciations.GetQueryable()
            .Where(d => d.PeriodMonth == monthStart.Month && d.PeriodYear == monthStart.Year)
            .Select(d => (double)d.Amount)
            .SumAsync();

        var overview = new RevenueOverviewDto
        {
            RevenueToday = (decimal)revToday,
            RevenueThisMonth = (decimal)revMonth,
            RevenueThisYear = (decimal)revYear,
            TotalExpenseThisMonth = (decimal)(matExpense + maintExpense + depExpense),
            NewMembersCount = await _unitOfWork.Members.GetQueryable().CountAsync(m => m.JoinedDate >= start && m.JoinedDate <= end),
            TotalPackagesSold = await _unitOfWork.Subscriptions.GetQueryable().CountAsync(s => s.CreatedAt >= start && s.CreatedAt <= end)
        };

        // 2. Revenue Chart (Selected Range)
        var pData = await paymentsQuery.Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
            .GroupBy(p => p.PaymentDate.Date)
            .Select(g => new { Date = g.Key, Amount = (decimal)g.Sum(x => (double)x.Amount) }).ToListAsync();
            
        var iData = await invoicesQuery.Where(i => i.CreatedAt >= start && i.CreatedAt <= end)
            .GroupBy(i => i.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Amount = (decimal)g.Sum(x => (double)(x.TotalAmount - x.DiscountAmount)) }).ToListAsync();

        var chartItems = Enumerable.Range(0, (end - start).Days + 1)
            .Select(i => {
                var d = start.AddDays(i).Date;
                var rev = (pData.FirstOrDefault(x => x.Date == d)?.Amount ?? 0) + 
                          (iData.FirstOrDefault(x => x.Date == d)?.Amount ?? 0);
                return new RevenueChartItemDto { Label = d.ToString("dd/MM"), Revenue = rev };
            }).ToList();

        // 3. Revenue by Package (Combining both systems)
        var pByPkg = await paymentsQuery
            .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
            .GroupBy(p => p.Subscription!.Package!.Name)
            .Select(g => new { Name = g.Key, Qty = g.Count(), Revenue = (decimal)g.Sum(p => (double)p.Amount) }).ToListAsync();

        var iByPkg = await invoicesQuery
            .Where(i => i.CreatedAt >= start && i.CreatedAt <= end)
            .SelectMany(i => i.Details)
            .Where(d => d.ItemType == "Package")
            .GroupBy(d => d.ItemName)
            .Select(g => new { Name = g.Key, Qty = g.Sum(d => d.Quantity), Revenue = (decimal)g.Sum(d => (double)(d.Quantity * d.UnitPrice)) }).ToListAsync();

        var allPkgNames = pByPkg.Select(x => x.Name).Union(iByPkg.Select(x => x.Name)).Distinct();
        var revByPackage = allPkgNames.Select(name => new RevenueByPackageDto
        {
            PackageName = name ?? "Khác",
            Quantity = (pByPkg.FirstOrDefault(x => x.Name == name)?.Qty ?? 0) + (iByPkg.FirstOrDefault(x => x.Name == name)?.Qty ?? 0),
            TotalRevenue = (pByPkg.FirstOrDefault(x => x.Name == name)?.Revenue ?? 0) + (iByPkg.FirstOrDefault(x => x.Name == name)?.Revenue ?? 0)
        }).OrderByDescending(x => x.TotalRevenue).ToList();

        // 4 & 5. Proxy reports for Trainer/Class (could be improved with actual Schedule logs)
        var trainers = await _unitOfWork.Trainers.GetQueryable().Include(t => t.Classes).ToListAsync();
        var revByTrainer = trainers.Select(t => new RevenueByTrainerDto
        {
            TrainerName = t.FullName,
            ClassesCount = t.Classes.Count,
            TotalRevenue = t.Classes.Sum(c => c.CurrentEnrollment) * 50000 
        }).OrderByDescending(x => x.TotalRevenue).ToList();

        // 6. Detailed Transactions
        var pTrans = await paymentsQuery.Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
            .OrderByDescending(p => p.PaymentDate).Take(30).ToListAsync();
        var iTrans = await invoicesQuery.Where(i => i.CreatedAt >= start && i.CreatedAt <= end)
            .OrderByDescending(i => i.CreatedAt).Take(30).ToListAsync();

        var transactions = pTrans.Select(p => new TransactionDetailDto {
            Id = p.Id, Date = p.PaymentDate, MemberName = p.Subscription?.Member?.FullName ?? "N/A",
            PackageName = p.Subscription?.Package?.Name ?? "HV Cũ", Amount = p.Amount, Status = "Completed",
            PaymentMethod = p.Method.ToString()
        }).Concat(iTrans.Select(i => new TransactionDetailDto {
            Id = i.Id, Date = i.CreatedAt, MemberName = i.Member?.FullName ?? "Khách lẻ/POS",
            PackageName = string.Join(", ", i.Details.Select(d => d.ItemName)), Amount = i.FinalAmount, Status = "Completed",
            PaymentMethod = i.PaymentMethod.ToString()
        })).OrderByDescending(t => t.Date).Take(50).ToList();

        var report = new RevenueReportDto
        {
            Overview = overview,
            RevenueChart = chartItems,
            RevenueByPackage = revByPackage,
            RevenueByTrainer = revByTrainer,
            RecentTransactions = transactions,
            TotalMaterialExpense = (decimal)matExpense,
            TotalMaintenanceExpense = (decimal)maintExpense,
            TotalDepreciationExpense = (decimal)depExpense
        };

        return ResponseDto<RevenueReportDto>.SuccessResult(report);
    }

    public async Task<ResponseDto<AssetInventoryStatsDto>> GetAssetInventoryStatsAsync()
    {
        var inventories = await _unitOfWork.Inventories.GetQueryable()
            .Include(i => i.Product)
            .ToListAsync();

        var equipments = await _unitOfWork.Equipments.GetQueryable()
            .Include(e => e.MaintenanceLogs)
            .Include(e => e.Depreciations)
            .ToListAsync();

        var stats = new AssetInventoryStatsDto
        {
            TotalProducts = inventories.Count,
            TotalStockItems = inventories.Sum(i => i.Quantity),
            TotalStockValue = inventories.Sum(i => i.Quantity * (i.Product?.Price ?? 0)),
            LowStockItems = inventories
                .Where(i => i.Quantity <= 5)
                .OrderBy(i => i.Quantity)
                .Take(5)
                .Select(i => new TopProductItemDto { Name = i.Product?.Name ?? "N/A", Quantity = i.Quantity })
                .ToList(),

            TotalEquipments = equipments.Count,
            TotalOriginalValue = equipments.Sum(e => e.PurchasePrice),
            TotalMaintenanceCosts = equipments.Sum(e => e.MaintenanceLogs.Sum(m => m.Cost)),
            StatusCounts = equipments
                .GroupBy(e => (int)e.Status)
                .Select(g => new EquipmentStatusCountDto { 
                    Status = g.Key == 1 ? "Hoạt động" : (g.Key == 2 ? "Hỏng" : "Bảo trì"), 
                    Count = g.Count() 
                })
                .ToList()
        };

        var totalDepreciation = equipments.Sum(e => e.AccumulatedDepreciation);
        stats.TotalCurrentValue = equipments.Sum(e => e.RemainingValue);

        return ResponseDto<AssetInventoryStatsDto>.SuccessResult(stats);
    }

    public async Task<ResponseDto<DepreciationReportDto>> GetDepreciationReportAsync(int month, int year)
    {
        var depreciations = await _unitOfWork.Depreciations.GetQueryable()
            .Include(d => d.Equipment)
                .ThenInclude(e => e!.EquipmentCategory)
            .Where(d => d.PeriodMonth == month && d.PeriodYear == year && !d.IsDeleted)
            .ToListAsync();

        var report = new DepreciationReportDto
        {
            Month = month,
            Year = year,
            TotalDepreciationAmount = depreciations.Sum(d => d.Amount),
            EquipmentCount = depreciations.Select(d => d.EquipmentId).Distinct().Count(),
            
            ByCategory = depreciations
                .GroupBy(d => d.Equipment!.EquipmentCategory?.Name ?? "Khác")
                .Select(g => new DepreciationByCategoryDto
                {
                    CategoryName = g.Key,
                    EquipmentCount = g.Select(d => d.EquipmentId).Distinct().Count(),
                    TotalAmount = g.Sum(d => d.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList(),
                
            Details = depreciations.Select(d => new EquipmentDepreciationDetailDto
            {
                EquipmentId = d.EquipmentId,
                EquipmentCode = d.Equipment!.EquipmentCode,
                EquipmentName = d.Equipment!.Name,
                OriginalPrice = d.Equipment!.PurchasePrice,
                DepreciationAmount = d.Amount,
                RemainingValue = d.RemainingValue,
                Status = d.Equipment!.Status.ToString()
            }).ToList()
        };

        return ResponseDto<DepreciationReportDto>.SuccessResult(report);
    }


    public async Task<ResponseDto<OperatingCostReportDto>> GetOperatingCostReportAsync(int month, int year)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddSeconds(-1);

        // 1. CHI PHÍ VẬT TƯ (Module Kho) - Lấy các giao dịch Xuất dùng, Hỏng, Hao hụt
        var materialTransactions = await _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .Where(t => t.Date >= startDate && t.Date <= endDate && 
                       (t.Type == StockTransactionType.InternalUse || 
                        t.Type == StockTransactionType.Damage || 
                        t.Type == StockTransactionType.Loss))
            .ToListAsync();

        var materialDetails = materialTransactions
            .GroupBy(t => t.Product?.Name ?? "Khác")
            .Select(g => new MaterialCostDetailDto
            {
                ProductName = g.Key,
                Quantity = g.Sum(t => t.Quantity),
                Cost = g.Sum(t => t.Quantity * (t.Product?.CostPrice ?? 0))
            }).ToList();

        // 2. CHI PHÍ KHẤU HAO (Module Thiết bị)
        var depreciations = await _unitOfWork.Depreciations.GetQueryable()
            .Include(d => d.Equipment)
            .Where(d => d.PeriodMonth == month && d.PeriodYear == year)
            .ToListAsync();

        var depreciationDetails = depreciations
            .GroupBy(d => d.Equipment?.Name ?? "Khác")
            .Select(g => new DepreciationCostDetailDto
            {
                EquipmentName = g.Key,
                Count = g.Select(d => d.EquipmentId).Distinct().Count(),
                Amount = g.Sum(d => d.Amount)
            }).ToList();

        var report = new OperatingCostReportDto
        {
            Month = month,
            Year = year,
            TotalMaterialCost = materialDetails.Sum(d => d.Cost),
            MaterialDetails = materialDetails,
            TotalDepreciationCost = depreciationDetails.Sum(d => d.Amount),
            DepreciationDetails = depreciationDetails
        };

        return ResponseDto<OperatingCostReportDto>.SuccessResult(report);
    }

    public async Task<ResponseDto<bool>> SeedReportDataAsync()
    {
        try
        {
            var random = new Random();
            var today = DateTime.UtcNow.Date;

            // #region agent log
            try
            {
                var logLine = System.Text.Json.JsonSerializer.Serialize(new
                {
                    sessionId = "43d464",
                    runId = "pre-fix",
                    hypothesisId = "H1",
                    location = "ReportsService.cs:SeedReportDataAsync",
                    message = "SeedReportDataAsync started",
                    data = new { now = DateTime.UtcNow },
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                }) + Environment.NewLine;
                System.IO.File.AppendAllText("debug-43d464.log", logLine);
            }
            catch
            {
                // ignored
            }
            // #endregion

            // 1. Ensure we have Packages
            var packages = await _unitOfWork.Packages.GetAllAsync();
            if (!packages.Any())
            {
                var p1 = new MembershipPackage { Name = "Gói 1 Tháng", Price = 500000, DurationDays = 30, IsActive = true };
                var p2 = new MembershipPackage { Name = "Gói 3 Tháng", Price = 1350000, DurationDays = 90, IsActive = true };
                var p3 = new MembershipPackage { Name = "Gói 1 Năm", Price = 4800000, DurationDays = 365, IsActive = true };
                await _unitOfWork.Packages.AddAsync(p1);
                await _unitOfWork.Packages.AddAsync(p2);
                await _unitOfWork.Packages.AddAsync(p3);
                await _unitOfWork.SaveChangesAsync();
                packages = new List<MembershipPackage> { p1, p2, p3 };
            }

            // 2. Ensure we have Trainers
            var trainers = await _unitOfWork.Trainers.GetAllAsync();
            if (!trainers.Any())
            {
                var t1 = new Trainer { FullName = "HLV Lê Văn Nam", Specialization = "Boxing/Kickboxing", ExperienceYears = 5 };
                var t2 = new Trainer { FullName = "HLV Trần Thị Hương", Specialization = "Yoga/Pilates", ExperienceYears = 7 };
                await _unitOfWork.Trainers.AddAsync(t1);
                await _unitOfWork.Trainers.AddAsync(t2);
                await _unitOfWork.SaveChangesAsync();
                trainers = new List<Trainer> { t1, t2 };

                // #region agent log
                try
                {
                    var logLine = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        sessionId = "43d464",
                        runId = "pre-fix",
                        hypothesisId = "H2",
                        location = "ReportsService.cs:SeedReportDataAsync",
                        message = "Seeded trainers with experience years",
                        data = new
                        {
                            t1 = new { t1.FullName, t1.Specialization, t1.ExperienceYears },
                            t2 = new { t2.FullName, t2.Specialization, t2.ExperienceYears }
                        },
                        timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    }) + Environment.NewLine;
                    System.IO.File.AppendAllText("debug-43d464.log", logLine);
                }
                catch
                {
                    // ignored
                }
                // #endregion
            }

            // 3. Ensure we have Classes
            var classes = await _unitOfWork.Classes.GetAllAsync();
            if (!classes.Any())
            {
                var c1 = new Class { ClassName = "Yoga Cơ Bản", TrainerId = trainers.First(t => t.FullName.Contains("Hương")).Id, MaxCapacity = 20, CurrentEnrollment = 15 };
                var c2 = new Class { ClassName = "Boxing Cường Độ Cao", TrainerId = trainers.First(t => t.FullName.Contains("Nam")).Id, MaxCapacity = 15, CurrentEnrollment = 12 };
                var c3 = new Class { ClassName = "Pilates Trẻ Hóa", TrainerId = trainers.First(t => t.FullName.Contains("Hương")).Id, MaxCapacity = 10, CurrentEnrollment = 8 };
                await _unitOfWork.Classes.AddAsync(c1);
                await _unitOfWork.Classes.AddAsync(c2);
                await _unitOfWork.Classes.AddAsync(c3);
                await _unitOfWork.SaveChangesAsync();
            }

            // 4. Create Members & Subscriptions & Payments for last 30 days
            var memberNames = new[] { "Nguyễn Văn A", "Trần Thị B", "Lê Văn C", "Phạm Minh D", "Võ Hoàng E", "Đặng Ngọc F", "Bùi Tiến G", "Hoàng Văn H" };

            // 🌟 THÊM: Hội viên mẫu cho FaceID để test ở local
            var faceTestMember = new Member
            {
                FullName = "FaceID Test Member",
                MemberCode = "GYM-FACE-001",
                FaceEncoding = "MOCK_FACE_VECTOR_GYM2024001", // Mã khớp với giả lập Frontend
                JoinedDate = today.AddMonths(-1),
                Status = MemberStatus.Active
            };
            await _unitOfWork.Members.AddAsync(faceTestMember);
            await _unitOfWork.SaveChangesAsync();

            var faceSub = new MemberSubscription
            {
                MemberId = faceTestMember.Id,
                PackageId = packages.First().Id,
                StartDate = today.AddMonths(-1),
                EndDate = today.AddMonths(11), // 1 năm
                Status = SubscriptionStatus.Active,
                CreatedAt = today.AddMonths(-1)
            };
            await _unitOfWork.Subscriptions.AddAsync(faceSub);
            await _unitOfWork.SaveChangesAsync();
            
            for (int i = 0; i < 30; i++)
            {
                var date = today.AddDays(-i);
                int transactionsToday = random.Next(1, 5); // 1-4 transactions per day

                for (int j = 0; j < transactionsToday; j++)
                {
                    var member = new Member
                    {
                        FullName = memberNames[random.Next(memberNames.Length)] + " " + i + j,
                        MemberCode = "M" + DateTime.UtcNow.Ticks.ToString().Substring(10),
                        JoinedDate = date,
                        Status = MemberStatus.Active
                    };
                    await _unitOfWork.Members.AddAsync(member);
                    await _unitOfWork.SaveChangesAsync();

                    var pkg = packages.ElementAt(random.Next(packages.Count()));
                    var sub = new MemberSubscription
                    {
                        MemberId = member.Id,
                        PackageId = pkg.Id,
                        StartDate = date,
                        EndDate = date.AddDays(pkg.DurationDays),
                        Status = SubscriptionStatus.Active,
                        CreatedAt = date
                    };
                    await _unitOfWork.Subscriptions.AddAsync(sub);
                    await _unitOfWork.SaveChangesAsync();

                    var payment = new Payment
                    {
                        MemberSubscriptionId = sub.Id,
                        Amount = pkg.Price,
                        PaymentDate = date.AddHours(random.Next(8, 20)),
                        Status = PaymentStatus.Completed,
                        Method = (PaymentMethod)random.Next(0, 3),
                        TransactionId = "TXN" + random.Next(100000, 999999)
                    };
                    await _unitOfWork.Payments.AddAsync(payment);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<bool>.SuccessResult(true, "Seed dữ liệu báo cáo thành công!");
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult("Lỗi seed dữ liệu: " + ex.Message);
        }
    }
}

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

        var start = startDate?.Date ?? today.AddDays(-29);
        var end = (endDate ?? today).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        try 
        {
            var payments = await _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted)
                .Include(p => p.Subscription).ThenInclude(s => s!.Package)
                .Include(p => p.Subscription).ThenInclude(s => s!.Member)
                .ToListAsync();

            var invoices = await _unitOfWork.Invoices.GetQueryable()
                .Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted)
                .Include(i => i.Member)
                .Include(i => i.Details)
                .ToListAsync();

            var orders = await _unitOfWork.Orders.GetQueryable()
                .Where(o => (o.Status == "Completed" || o.Status == "completed") && !o.IsDeleted)
                .Include(o => o.Member)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .ToListAsync();

            // 1. Overview
            var revMonth = payments.Where(p => p.PaymentDate >= monthStart).Sum(p => (double)p.Amount) +
                           invoices.Where(i => i.CreatedAt >= monthStart).Sum(i => (double)i.FinalAmount) +
                           orders.Where(o => o.CreatedDate >= monthStart).Sum(o => (double)o.TotalAmount);
                           
            // Expenses
            var matExpense = (await _unitOfWork.StockTransactions.GetQueryable()
                .Include(t => t.Product)
                .Where(t => t.Date >= monthStart && (t.Type == StockTransactionType.InternalUse || t.Type == StockTransactionType.Damage || t.Type == StockTransactionType.Loss))
                .ToListAsync()).Sum(t => (double)(t.Quantity * (t.Product?.CostPrice ?? 0)));
            
            var maintExpense = (await _unitOfWork.MaintenanceLogs.GetQueryable()
                .Where(m => m.Date >= monthStart && m.Status == MaintenanceStatus.Completed)
                .ToListAsync()).Sum(m => (double)m.Cost);
                
            var depExpense = (await _unitOfWork.Depreciations.GetQueryable()
                .Where(d => d.PeriodMonth == monthStart.Month && d.PeriodYear == monthStart.Year)
                .ToListAsync()).Sum(d => (double)d.Amount);

            var overview = new RevenueOverviewDto
            {
                RevenueToday = (decimal)(payments.Where(p => p.PaymentDate.Date == today).Sum(p => (double)p.Amount) + invoices.Where(p => p.CreatedAt.Date == today).Sum(p => (double)p.FinalAmount)),
                RevenueThisMonth = (decimal)revMonth,
                RevenueThisYear = (decimal)(payments.Where(p => p.PaymentDate >= yearStart).Sum(p => (double)p.Amount)),
                TotalExpenseThisMonth = (decimal)(matExpense + maintExpense + depExpense),
                NewMembersCount = await _unitOfWork.Members.GetQueryable().CountAsync(m => m.JoinedDate >= start && m.JoinedDate <= end),
                TotalPackagesSold = await _unitOfWork.Subscriptions.GetQueryable().CountAsync(s => s.CreatedAt >= start && s.CreatedAt <= end)
            };

            // 2. Charts
            var chartItems = Enumerable.Range(0, (end.Date - start.Date).Days + 1)
                .Select(i => {
                    var d = start.Date.AddDays(i);
                    var pRev = payments.Where(p => p.PaymentDate.Date == d).Sum(p => (decimal)p.Amount);
                    var iRev = invoices.Where(i => i.CreatedAt.Date == d).Sum(i => (decimal)i.FinalAmount);
                    var oRev = orders.Where(o => o.CreatedDate.Date == d).Sum(o => (decimal)o.TotalAmount);
                    return new RevenueChartItemDto { Label = d.ToString("dd/MM"), Revenue = pRev + iRev + oRev };
                }).ToList();

            // 3. Category Breakdown
            var revByPackage = payments
                .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
                .GroupBy(p => p.Subscription?.Package?.Name ?? "Dịch vụ Membership")
                .Select(g => new RevenueByPackageDto { PackageName = g.Key, Quantity = g.Count(), TotalRevenue = (decimal)g.Sum(p => p.Amount) })
                .OrderByDescending(x => x.TotalRevenue).ToList();

            var revByProduct = invoices
                .Where(i => i.CreatedAt >= start && i.CreatedAt <= end)
                .SelectMany(i => i.Details)
                .Where(d => d.ItemType == "Product")
                .GroupBy(d => d.ItemName)
                .Select(g => new RevenueByPackageDto { PackageName = g.Key, Quantity = g.Sum(x => x.Quantity), TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice) })
                .OrderByDescending(x => x.TotalRevenue).ToList();

            // 4. Hourly Distribution (Real Data)
            var allTrans = payments.Select(p => new { Date = p.PaymentDate, Amount = (decimal)p.Amount })
                .Concat(invoices.Select(i => new { Date = i.CreatedAt, Amount = (decimal)i.FinalAmount }))
                .Concat(orders.Select(o => new { Date = o.CreatedDate, Amount = o.TotalAmount }))
                .Where(x => x.Date >= start && x.Date <= end)
                .ToList();

            var revByHour = allTrans
                .GroupBy(x => x.Date.Hour)
                .Select(g => new RevenueByHourDto {
                    Hour = $"{g.Key:D2}:00",
                    Revenue = g.Sum(x => x.Amount),
                    TransactionCount = g.Count()
                })
                .OrderBy(x => x.Hour)
                .ToList();

            // 5. Recent Transactions (Real Detailed Data)
            var transactions = payments
                .Select(p => new TransactionDetailDto {
                    Date = p.PaymentDate,
                    Amount = (decimal)p.Amount,
                    MemberName = p.Subscription?.Member?.FullName ?? "N/A",
                    PackageName = "Gói: " + (p.Subscription?.Package?.Name ?? "Dịch vụ"),
                    Status = "Completed"
                })
                .Concat(invoices.Select(i => new TransactionDetailDto {
                    Date = i.CreatedAt,
                    Amount = (decimal)i.FinalAmount,
                    MemberName = i.Member?.FullName ?? "Khách lẻ/POS",
                    PackageName = "Sản phẩm/Dịch vụ lẻ",
                    Status = "Completed"
                }))
                .Concat(orders.Select(o => new TransactionDetailDto {
                    Date = o.CreatedDate,
                    Amount = o.TotalAmount,
                    MemberName = o.Member?.FullName ?? "Đơn hàng Web",
                    PackageName = "Sản phẩm từ Store",
                    Status = "Completed"
                }))
                .OrderByDescending(x => x.Date)
                .Take(100)
                .ToList();

            return ResponseDto<RevenueReportDto>.SuccessResult(new RevenueReportDto {
                Overview = overview,
                RevenueChart = chartItems,
                RevenueByPackage = revByPackage,
                RevenueByProduct = revByProduct,
                RevenueByHour = revByHour,
                RecentTransactions = transactions,
                TotalMaterialExpense = (decimal)matExpense,
                TotalMaintenanceExpense = (decimal)maintExpense,
                TotalDepreciationExpense = (decimal)depExpense
            });
        }
        catch (Exception ex)
        {
            return ResponseDto<RevenueReportDto>.FailureResult($"Lỗi báo cáo: {ex.Message}");
        }
    }

    public async Task<ResponseDto<AssetInventoryStatsDto>> GetAssetInventoryStatsAsync() { return null; }
    public async Task<ResponseDto<DepreciationReportDto>> GetDepreciationReportAsync(int month, int year) { return null; }
    public async Task<ResponseDto<OperatingCostReportDto>> GetOperatingCostReportAsync(int month, int year) { return null; }
    public async Task<ResponseDto<bool>> SeedReportDataAsync() { return null; }
}

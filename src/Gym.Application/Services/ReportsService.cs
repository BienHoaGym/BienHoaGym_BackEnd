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
using MiniExcelLibs;
using System.IO;

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
        var start = startDate?.Date ?? DateTime.UtcNow.Date.AddDays(-29);
        var end = (endDate?.Date ?? DateTime.UtcNow.Date).AddHours(23).AddMinutes(59).AddSeconds(59);

        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var yearStart = new DateTime(now.Year, 1, 1);
        var today = now.Date;

        try 
        {
            var payments = await _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted && p.PaymentDate >= start && p.PaymentDate <= end)
                .Include(p => p.Subscription).ThenInclude(s => s!.Package)
                .Include(p => p.Subscription).ThenInclude(s => s!.Member)
                .ToListAsync();

            var invoices = await _unitOfWork.Invoices.GetQueryable()
                .Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted && i.CreatedAt >= start && i.CreatedAt <= end)
                .Include(i => i.Member)
                .Include(i => i.Details)
                .ToListAsync();

            var orders = await _unitOfWork.Orders.GetQueryable()
                .Where(o => (o.Status == "Completed" || o.Status == "completed") && !o.IsDeleted && o.CreatedDate >= start && o.CreatedDate <= end)
                .Include(o => o.Member)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .ToListAsync();

            var revenueThisMonth = await _unitOfWork.Payments.GetQueryable().Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted && p.PaymentDate >= monthStart).SumAsync(p => (decimal)p.Amount) +
                                   await _unitOfWork.Invoices.GetQueryable().Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted && i.CreatedAt >= monthStart).SumAsync(i => (decimal)i.FinalAmount);

            var revenueThisYear = await _unitOfWork.Payments.GetQueryable().Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted && p.PaymentDate >= yearStart).SumAsync(p => (decimal)p.Amount);

            var maintExpenseMonth = await _unitOfWork.MaintenanceLogs.GetQueryable().Where(m => m.Date >= monthStart && m.Status == MaintenanceStatus.Completed).SumAsync(m => (decimal)m.Cost);
            var depExpenseMonth = await _unitOfWork.Depreciations.GetQueryable().Where(d => d.PeriodMonth == now.Month && d.PeriodYear == now.Year).SumAsync(d => (decimal)d.Amount);

            var overview = new RevenueOverviewDto
            {
                RevenueToday = (decimal)(payments.Where(p => p.PaymentDate.Date == today).Sum(p => (double)p.Amount) + 
                                       invoices.Where(i => i.CreatedAt.Date == today).Sum(i => (double)i.FinalAmount)),
                RevenueThisMonth = revenueThisMonth,
                RevenueThisYear = revenueThisYear,
                TotalExpenseThisMonth = maintExpenseMonth + depExpenseMonth,
                NewMembersCount = await _unitOfWork.Members.GetQueryable().CountAsync(m => !m.IsDeleted && m.JoinedDate >= start && m.JoinedDate <= end),
                TotalPackagesSold = await _unitOfWork.Subscriptions.GetQueryable().CountAsync(s => !s.IsDeleted && s.CreatedAt >= start && s.CreatedAt <= end)
            };

            var chartItems = Enumerable.Range(0, (end.Date - start.Date).Days + 1)
                .Select(i => {
                    var d = start.Date.AddDays(i);
                    var pRev = payments.Where(p => p.PaymentDate.Date == d).Sum(p => (decimal)p.Amount);
                    var iRev = invoices.Where(i => i.CreatedAt.Date == d).Sum(i => (decimal)i.FinalAmount);
                    var oRev = orders.Where(o => o.CreatedDate.Date == d).Sum(o => (decimal)o.TotalAmount);
                    return new RevenueChartItemDto { Label = d.ToString("dd/MM"), Revenue = pRev + iRev + oRev };
                }).ToList();

            var revByPackage = payments
                .GroupBy(p => p.Subscription?.Package?.Name ?? "D\u1ECBch v\u1EE5 Membership")
                .Select(g => new RevenueByPackageDto { PackageName = g.Key, Quantity = g.Count(), TotalRevenue = (decimal)g.Sum(p => p.Amount) })
                .OrderByDescending(x => x.TotalRevenue).ToList();

            var revByProduct = invoices
                .SelectMany(i => i.Details)
                .Where(d => d.ItemType == "Product")
                .GroupBy(d => d.ItemName)
                .Select(g => new RevenueByPackageDto { PackageName = g.Key, Quantity = g.Sum(x => x.Quantity), TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice) })
                .OrderByDescending(x => x.TotalRevenue).ToList();

            var revByHour = payments.Select(p => new { Date = p.PaymentDate, Amount = (decimal)p.Amount })
                .Concat(invoices.Select(i => new { Date = i.CreatedAt, Amount = (decimal)i.FinalAmount }))
                .Concat(orders.Select(o => new { Date = o.CreatedDate, Amount = o.TotalAmount }))
                .GroupBy(x => x.Date.Hour)
                .Select(g => new RevenueByHourDto {
                    Hour = $"{g.Key:D2}:00",
                    Revenue = g.Sum(x => x.Amount),
                    TransactionCount = g.Count()
                })
                .OrderBy(x => x.Hour)
                .ToList();

            var transactions = payments
                .Select(p => new TransactionDetailDto {
                    Date = p.PaymentDate,
                    Amount = (decimal)p.Amount,
                    MemberName = p.Subscription?.Member?.FullName ?? "N/A",
                    PackageName = "G\u00F3i: " + (p.Subscription?.Package?.Name ?? "D\u1ECBch v\u1EE5"),
                    Status = "Completed"
                })
                .Concat(invoices.Select(i => new TransactionDetailDto {
                    Date = i.CreatedAt,
                    Amount = (decimal)i.FinalAmount,
                    MemberName = i.Member?.FullName ?? "Kh\u00E1ch l\u1EBB/POS",
                    PackageName = "S\u1EA3n ph\u1EA9m/D\u1ECBch v\u1EE5 l\u1EBB",
                    Status = "Completed"
                }))
                .Concat(orders.Select(o => new TransactionDetailDto {
                    Date = o.CreatedDate,
                    Amount = o.TotalAmount,
                    MemberName = o.Member?.FullName ?? "\u0110\u01A1n h\u00E0ng Web",
                    PackageName = "S\u1EA3n ph\u1EA9m t\u1EEB Store",
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
                RecentTransactions = transactions
            });
        }
        catch (Exception ex)
        {
            return ResponseDto<RevenueReportDto>.FailureResult($"L\u1ED7i b\u00E1o c\u00E1o: {ex.Message}");
        }
    }

    public async Task<byte[]> ExportRevenueToExcelAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var reportResponse = await GetRevenueReportAsync(startDate, endDate);
        if (!reportResponse.Success || reportResponse.Data == null)
            return Array.Empty<byte>();

        var data = reportResponse.Data!;
        
        // Chuẩn bị dữ liệu xuất Excel
        var excelData = data.RecentTransactions.Select(t => new {
            Ngay = t.Date.ToString("dd/MM/yyyy HH:mm"),
            KhachHang = t.MemberName,
            NoiDung = t.PackageName,
            SoTien = t.Amount,
            TrangThai = t.Status
        }).ToList();

        using (var ms = new MemoryStream())
        {
            await ms.SaveAsAsync(excelData);
            return ms.ToArray();
        }
    }

    public Task<ResponseDto<AssetInventoryStatsDto>> GetAssetInventoryStatsAsync()
    {
        return Task.FromResult(ResponseDto<AssetInventoryStatsDto>.SuccessResult(new AssetInventoryStatsDto()));
    }

    public Task<ResponseDto<DepreciationReportDto>> GetDepreciationReportAsync(int month, int year)
    {
        return Task.FromResult(ResponseDto<DepreciationReportDto>.SuccessResult(new DepreciationReportDto { Month = month, Year = year }));
    }

    public Task<ResponseDto<OperatingCostReportDto>> GetOperatingCostReportAsync(int month, int year)
    {
        return Task.FromResult(ResponseDto<OperatingCostReportDto>.SuccessResult(new OperatingCostReportDto { Month = month, Year = year }));
    }

    public Task<ResponseDto<bool>> SeedReportDataAsync()
    {
        return Task.FromResult(ResponseDto<bool>.SuccessResult(true, "Ch\u1EE9c n\u0103ng Seed d\u1EE7 li\u1EC7u \u0111\u00E3 ho\u00E0n t\u1EA5t th\u00F4ng qua Program startup."));
    }
}

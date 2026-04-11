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
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Gym.Application.Services;

public class ReportsService : IReportsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
            Console.WriteLine($"🔍 Generating report from {start} to {end}...");

            var paymentsQuery = _unitOfWork.Payments.GetQueryable()
                .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted && p.PaymentDate >= start && p.PaymentDate <= end);
            
            var payments = await paymentsQuery
                .Include(p => p.Subscription).ThenInclude(s => s!.Package)
                .Include(p => p.Subscription).ThenInclude(s => s!.Member)
                .ToListAsync();
            Console.WriteLine($"✅ Found {payments.Count} payments");

            var invoicesQuery = _unitOfWork.Invoices.GetQueryable()
                .Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted && i.CreatedAt >= start && i.CreatedAt <= end);

            var invoices = await invoicesQuery
                .Include(i => i.Member)
                .Include(i => i.Details)
                .ToListAsync();
            Console.WriteLine($"✅ Found {invoices.Count} invoices");

            var ordersQuery = _unitOfWork.Orders.GetQueryable()
                .Where(o => (o.Status == "Completed" || o.Status == "completed") && !o.IsDeleted && o.CreatedDate >= start && o.CreatedDate <= end);

            var orders = await ordersQuery
                .Include(o => o.Member)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .ToListAsync();
            Console.WriteLine($"✅ Found {orders.Count} orders");

            // Tính toán tổng doanh thu trong KỲ ĐANG XEM (periodRevenue) thay vì cứng thán hiện tại
            var periodRevenue = payments.Sum(p => p.Amount) + 
                               invoices.Sum(i => i.TotalAmount - i.DiscountAmount) +
                               orders.Sum(o => o.TotalAmount);

            var rawPaymentsThisMonth = await _unitOfWork.Payments.GetQueryable().Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted && p.PaymentDate >= monthStart).Select(p => p.Amount).ToListAsync();
            var rawInvoicesThisMonth = await _unitOfWork.Invoices.GetQueryable().Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted && i.CreatedAt >= monthStart).Select(i => i.TotalAmount - i.DiscountAmount).ToListAsync();
            var rawOrdersThisMonth = await _unitOfWork.Orders.GetQueryable().Where(o => (o.Status == "Completed" || o.Status == "completed") && !o.IsDeleted && o.CreatedDate >= monthStart).Select(o => o.TotalAmount).ToListAsync();
            var revenueThisMonth = rawPaymentsThisMonth.Sum() + rawInvoicesThisMonth.Sum() + rawOrdersThisMonth.Sum();

            var rawPaymentsThisYear = await _unitOfWork.Payments.GetQueryable().Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted && p.PaymentDate >= yearStart).Select(p => p.Amount).ToListAsync();
            var rawInvoicesThisYear = await _unitOfWork.Invoices.GetQueryable().Where(i => i.Status == PaymentStatus.Completed && !i.IsDeleted && i.CreatedAt >= yearStart).Select(i => i.TotalAmount - i.DiscountAmount).ToListAsync();
            var rawOrdersThisYear = await _unitOfWork.Orders.GetQueryable().Where(o => (o.Status == "Completed" || o.Status == "completed") && !o.IsDeleted && o.CreatedDate >= yearStart).Select(o => o.TotalAmount).ToListAsync();
            var revenueThisYear = rawPaymentsThisYear.Sum() + rawInvoicesThisYear.Sum() + rawOrdersThisYear.Sum();

            var rawMaintExpense = await _unitOfWork.MaintenanceLogs.GetQueryable().Where(m => m.Date >= start && m.Date <= end && m.Status == MaintenanceStatus.Completed).Select(m => m.Cost).ToListAsync();
            var maintExpenseMonth = rawMaintExpense.Sum();
            
            var rawDepExpense = await _unitOfWork.Depreciations.GetQueryable().Where(d => d.PeriodMonth == now.Month && d.PeriodYear == now.Year).Select(d => d.Amount).ToListAsync();
            var depExpenseMonth = rawDepExpense.Sum();

            var overview = new RevenueOverviewDto
            {
                RevenueToday = payments.Where(p => p.PaymentDate.Date == today).Sum(p => p.Amount) + 
                                       invoices.Where(i => i.CreatedAt.Date == today).Sum(i => i.TotalAmount - i.DiscountAmount) +
                                       orders.Where(o => o.CreatedDate.Date == today).Sum(o => o.TotalAmount),
                RevenueThisMonth = periodRevenue, // Trả về doanh thu trong kỳ được chọn để hiển thị lên KPI Cards
                RevenueThisYear = revenueThisYear,
                TotalExpenseThisMonth = maintExpenseMonth + depExpenseMonth,
                NewMembersCount = await _unitOfWork.Members.GetQueryable().CountAsync(m => !m.IsDeleted && m.JoinedDate >= start && m.JoinedDate <= end),
                TotalPackagesSold = await _unitOfWork.Subscriptions.GetQueryable().CountAsync(s => !s.IsDeleted && s.CreatedAt >= start && s.CreatedAt <= end)
            };

            var chartItems = Enumerable.Range(0, (end.Date - start.Date).Days + 1)
                .Select(i => {
                    var d = start.Date.AddDays(i);
                    var pRev = payments.Where(p => p.PaymentDate.Date == d).Sum(p => p.Amount);
                    var iRev = invoices.Where(i => i.CreatedAt.Date == d).Sum(i => i.TotalAmount - i.DiscountAmount);
                    var oRev = orders.Where(o => o.CreatedDate.Date == d).Sum(o => o.TotalAmount);
                    return new RevenueChartItemDto { Label = d.ToString("dd/MM"), Revenue = pRev + iRev + oRev };
                }).ToList();

            var revByPackage = payments
                .GroupBy(p => p.Subscription?.Package?.Name ?? "D\u1ECBch v\u1EE5 Membership")
                .Select(g => new RevenueByPackageDto { PackageName = g.Key, Quantity = g.Count(), TotalRevenue = g.Sum(p => p.Amount) })
                .OrderByDescending(x => x.TotalRevenue).ToList();

            var revByProduct = invoices
                .SelectMany(i => i.Details)
                .Where(d => d.ItemType == "Product")
                .GroupBy(d => d.ItemName)
                .Select(g => new RevenueByPackageDto { PackageName = g.Key, Quantity = g.Sum(x => x.Quantity), TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice) })
                .OrderByDescending(x => x.TotalRevenue).ToList();

            var revByHour = payments.Select(p => new { Date = p.PaymentDate, Amount = p.Amount })
                .Concat(invoices.Select(i => new { Date = i.CreatedAt, Amount = i.TotalAmount - i.DiscountAmount }))
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
                    Amount = p.Amount,
                    MemberName = p.Subscription?.Member?.FullName ?? "N/A",
                    PackageName = "G\u00F3i: " + (p.Subscription?.Package?.Name ?? "D\u1ECBch v\u1EE5"),
                    Status = "Completed"
                })
                .Concat(invoices.Select(i => new TransactionDetailDto {
                    Date = i.CreatedAt,
                    Amount = i.TotalAmount - i.DiscountAmount,
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

    public async Task<ResponseDto<AssetInventoryStatsDto>> GetAssetInventoryStatsAsync()
    {
        try
        {
            var products = await _unitOfWork.Products.GetQueryable()
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            var equipments = await _unitOfWork.Equipments.GetQueryable()
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            var maintenanceLogs = await _unitOfWork.MaintenanceLogs.GetQueryable()
                .Where(m => !m.IsDeleted && m.Status == MaintenanceStatus.Completed)
                .ToListAsync();

            var stats = new AssetInventoryStatsDto
            {
                // Inventory
                TotalProducts = products.Count,
                TotalStockItems = products.Sum(p => p.StockQuantity),
                TotalStockValue = (decimal)products.Sum(p => (double)p.StockQuantity * (double)p.Price),
                LowStockItems = products.Where(p => p.StockQuantity < 10)
                    .OrderBy(p => p.StockQuantity)
                    .Take(5)
                    .Select(p => new TopProductItemDto { Name = p.Name, Quantity = p.StockQuantity })
                    .ToList(),

                // Equipment
                TotalEquipments = equipments.Count,
                TotalOriginalValue = equipments.Sum(e => e.PurchasePrice),
                TotalCurrentValue = equipments.Sum(e => e.RemainingValue),
                TotalMaintenanceCosts = maintenanceLogs.Sum(m => m.Cost),
                StatusCounts = equipments.GroupBy(e => e.Status.ToString())
                    .Select(g => new EquipmentStatusCountDto { Status = g.Key, Count = g.Count() })
                    .ToList()
            };

            return ResponseDto<AssetInventoryStatsDto>.SuccessResult(stats);
        }
        catch (Exception ex)
        {
            return ResponseDto<AssetInventoryStatsDto>.FailureResult($"L\u1ED7i th\u1ED1ng k\u00EA t\u00E0i s\u1EA3n: {ex.Message}");
        }
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
        return Task.FromResult(ResponseDto<bool>.SuccessResult(true, "Y\u00EAu c\u1EA7u seed \u0111\u00E3 \u0111\u01B0\u1EE3c nh\u1EADn."));
    }

    public async Task<object> GetDatabaseStatsAsync()
    {
        return new 
        {
            TotalMembers = await _unitOfWork.Members.GetQueryable().CountAsync(),
            TotalInvoices = await _unitOfWork.Invoices.GetQueryable().CountAsync(),
            CompletedInvoices = await _unitOfWork.Invoices.GetQueryable().CountAsync(i => i.Status == PaymentStatus.Completed),
            TotalPayments = await _unitOfWork.Payments.GetQueryable().CountAsync(),
            CompletedPayments = await _unitOfWork.Payments.GetQueryable().CountAsync(p => p.Status == PaymentStatus.Completed),
            TotalOrders = await _unitOfWork.Orders.GetQueryable().CountAsync(),
            CompletedOrders = await _unitOfWork.Orders.GetQueryable().CountAsync(o => o.Status == "Completed" || o.Status == "completed"),
            RecentInvoices = await _unitOfWork.Invoices.GetQueryable().OrderByDescending(i => i.CreatedAt).Take(5).Select(i => new { i.InvoiceNumber, i.CreatedAt, i.TotalAmount, Status = i.Status.ToString() }).ToListAsync()
        };
    }
}

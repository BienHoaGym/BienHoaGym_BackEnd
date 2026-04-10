using System;
using System.Collections.Generic;

namespace Gym.Application.DTOs.Reports;

public class RevenueOverviewDto
{
    public decimal RevenueToday { get; set; }
    public decimal RevenueThisMonth { get; set; }
    public decimal RevenueThisYear { get; set; }
    public decimal TotalExpenseThisMonth { get; set; } // Tổng chi phí (Kho + Thiết bị)
    public decimal NetProfitThisMonth => RevenueThisMonth - TotalExpenseThisMonth;

    public int NewMembersCount { get; set; }
    public int TotalPackagesSold { get; set; }
}

public class RevenueChartItemDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class RevenueByPackageDto
{
    public string PackageName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class RevenueByTrainerDto
{
    public string TrainerName { get; set; } = string.Empty;
    public int ClassesCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class RevenueByClassDto
{
    public string ClassName { get; set; } = string.Empty;
    public int MembersCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class TransactionDetailDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
}

public class RevenueByHourDto
{
    public string Hour { get; set; } = string.Empty; // e.g., "08:00"
    public decimal Revenue { get; set; }
    public int TransactionCount { get; set; }
}

public class RevenueReportDto
{
    public RevenueOverviewDto Overview { get; set; } = new();
    public List<RevenueChartItemDto> RevenueChart { get; set; } = new();
    public List<RevenueByPackageDto> RevenueByPackage { get; set; } = new();
    public List<RevenueByTrainerDto> RevenueByTrainer { get; set; } = new();
    public List<RevenueByClassDto> RevenueByClass { get; set; } = new();
    public List<RevenueByPackageDto> RevenueByProduct { get; set; } = new(); // For retail products
    public List<RevenueByHourDto> RevenueByHour { get; set; } = new(); // NEW: For hourly heatmaps
    public List<TransactionDetailDto> RecentTransactions { get; set; } = new();
    
    // Financial Analysis
    public decimal TotalMaterialExpense { get; set; }
    public decimal TotalMaintenanceExpense { get; set; }
    public decimal TotalDepreciationExpense { get; set; }
}

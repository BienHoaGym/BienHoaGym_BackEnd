using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Reports;

namespace Gym.Application.Interfaces.Services;

public interface IReportsService
{
    Task<ResponseDto<RevenueReportDto>> GetRevenueReportAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<byte[]> ExportRevenueToExcelAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<ResponseDto<AssetInventoryStatsDto>> GetAssetInventoryStatsAsync();
    Task<ResponseDto<DepreciationReportDto>> GetDepreciationReportAsync(int month, int year);
    Task<ResponseDto<OperatingCostReportDto>> GetOperatingCostReportAsync(int month, int year);
    Task<ResponseDto<bool>> SeedReportDataAsync();
}

using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Providers;

namespace Gym.Application.Interfaces.Services;

public interface IProviderService
{
    Task<ResponseDto<List<ProviderSummaryDto>>> GetAllSummariesAsync(string? searchTerm = null, string? supplyType = null);
    Task<ResponseDto<ProviderDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<ProviderDto>> CreateAsync(CreateProviderDto dto);
    Task<ResponseDto<ProviderDto>> UpdateAsync(Guid id, CreateProviderDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
    Task<ResponseDto<List<ProviderTransactionHistoryDto>>> GetTransactionHistoryAsync(Guid providerId);
}

using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Dashboard;

namespace Gym.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<ResponseDto<DashboardStatsDto>> GetStatsAsync();
}

using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface ICheckInRepository : IRepository<CheckIn>
{
    Task<List<CheckIn>> GetTodayCheckInsAsync();
    Task<List<CheckIn>> GetByMemberIdAsync(Guid memberId, int take = 10);
    Task<CheckIn?> GetActiveCheckInAsync(Guid memberId);
    Task<List<CheckIn>> GetCheckInsByDateRangeAsync(DateTime startDate, DateTime endDate);
}

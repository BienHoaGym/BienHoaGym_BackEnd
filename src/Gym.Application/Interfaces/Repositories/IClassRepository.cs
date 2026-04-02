using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface IClassRepository : IRepository<Class>
{
    Task<List<Class>> GetByTrainerIdAsync(Guid trainerId);
    Task<List<Class>> GetUpcomingClassesAsync();
}

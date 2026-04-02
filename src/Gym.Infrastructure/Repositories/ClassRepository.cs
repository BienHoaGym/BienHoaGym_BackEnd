using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class ClassRepository : Repository<Class>, IClassRepository
{
    public ClassRepository(GymDbContext context) : base(context)
    {
    }

    public override async Task<Class?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include(c => c.Trainer)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public override async Task<IEnumerable<Class>> GetAllAsync()
    {
        return await _dbSet.Include(c => c.Trainer)
            .ToListAsync();
    }

    public async Task<List<Class>> GetByTrainerIdAsync(Guid trainerId)
    {
        return await _dbSet.Include(c => c.Trainer)
            .Where(c => c.TrainerId == trainerId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Class>> GetUpcomingClassesAsync()
    {
        var currentTime = DateTime.UtcNow.TimeOfDay;
        return await _dbSet.Include(c => c.Trainer)
            .Where(c => !c.IsDeleted && c.StartTime >= currentTime)
            .OrderBy(c => c.StartTime)
            .ToListAsync();
    }
}

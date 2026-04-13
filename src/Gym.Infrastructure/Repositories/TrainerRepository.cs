using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    public TrainerRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<List<Trainer>> GetAvailableTrainersAsync()
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => !t.IsDeleted && t.IsActive && t.IsPublic)
            .OrderBy(t => t.FullName)
            .ToListAsync();
    }

    public async Task<Trainer?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Classes)
            .Include(t => t.TrainerMemberAssignments)
                .ThenInclude(a => a.Member)
            .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);
    }
}

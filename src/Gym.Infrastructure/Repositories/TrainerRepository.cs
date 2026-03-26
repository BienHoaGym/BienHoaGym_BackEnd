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
        var allTrainers = await GetAllAsync();
        return allTrainers.Where(t => !t.IsDeleted && t.IsActive).OrderBy(t => t.FullName).ToList();
    }

    public async Task<Trainer?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.Classes)
            .Include(t => t.TrainerMemberAssignments)
                .ThenInclude(a => a.Member)
            .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);
    }
}
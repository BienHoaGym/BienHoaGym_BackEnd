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
}
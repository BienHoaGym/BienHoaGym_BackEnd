using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface ITrainerRepository : IRepository<Trainer>
{
    Task<List<Trainer>> GetAvailableTrainersAsync();
}

using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
}

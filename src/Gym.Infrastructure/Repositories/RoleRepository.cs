using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;

namespace Gym.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(GymDbContext context) : base(context)
    {
    }
}

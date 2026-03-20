using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface IPackageRepository : IRepository<MembershipPackage>
{
    Task<List<MembershipPackage>> GetActivePackagesAsync();
}


using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;

namespace Gym.Infrastructure.Repositories;

public class PackageRepository : Repository<MembershipPackage>, IPackageRepository
{
    public PackageRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<List<MembershipPackage>> GetActivePackagesAsync()
    {
        var allPackages = await GetAllAsync();
        return allPackages.Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToList();
    }
}


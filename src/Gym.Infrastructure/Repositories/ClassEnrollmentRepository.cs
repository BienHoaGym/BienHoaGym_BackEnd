using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;

namespace Gym.Infrastructure.Repositories;

public class ClassEnrollmentRepository : Repository<ClassEnrollment>, IClassEnrollmentRepository
{
    public ClassEnrollmentRepository(GymDbContext context) : base(context)
    {
    }
}

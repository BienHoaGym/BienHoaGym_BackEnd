using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class SubscriptionRepository : Repository<MemberSubscription>, ISubscriptionRepository
{
    public SubscriptionRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<List<MemberSubscription>> GetByMemberIdAsync(Guid memberId)
    {
        return await _dbSet.Where(s => s.MemberId == memberId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<MemberSubscription>> GetActiveSubscriptionsAsync()
    {
        return await _dbSet.Where(s => !s.IsDeleted && s.Status == Gym.Domain.Enums.SubscriptionStatus.Active)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }
}

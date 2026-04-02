using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface ISubscriptionRepository : IRepository<MemberSubscription>
{
    Task<List<MemberSubscription>> GetByMemberIdAsync(Guid memberId);
    Task<List<MemberSubscription>> GetActiveSubscriptionsAsync();
}

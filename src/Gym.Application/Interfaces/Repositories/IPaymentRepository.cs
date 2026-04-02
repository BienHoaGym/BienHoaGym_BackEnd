using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<List<Payment>> GetByMemberIdAsync(Guid memberId);
    Task<List<Payment>> GetBySubscriptionIdAsync(Guid subscriptionId);
}


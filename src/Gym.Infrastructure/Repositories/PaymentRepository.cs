using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<List<Payment>> GetByMemberIdAsync(Guid memberId)
    {
        return await _dbSet
            .Include(p => p.Subscription!).ThenInclude(s => s!.Package) // Nạp Package
            .Where(p => p.Subscription!.MemberId == memberId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<List<Payment>> GetBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await _dbSet
            .Include(p => p.Subscription!).ThenInclude(s => s!.Member)  // Nạp Member
            .Include(p => p.Subscription!).ThenInclude(s => s!.Package) // Nạp Package
            .Where(p => p.MemberSubscriptionId == subscriptionId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }
}
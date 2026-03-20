using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByMemberCodeAsync(string memberCode)
    {
        return await _dbSet.FirstOrDefaultAsync(m => m.MemberCode == memberCode && !m.IsDeleted);
    }

    public async Task<List<Member>> SearchAsync(string keyword)
    {
        return await _dbSet.Where(m => !m.IsDeleted &&
                       ($"{m.FullName}".Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        m.MemberCode.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(m.Email) && m.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        m.PhoneNumber.Contains(keyword)))
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }
}
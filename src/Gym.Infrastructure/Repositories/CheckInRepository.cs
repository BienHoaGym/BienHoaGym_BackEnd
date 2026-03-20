using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class CheckInRepository : Repository<CheckIn>, ICheckInRepository
{
    public CheckInRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<List<CheckIn>> GetTodayCheckInsAsync()
    {
        var today = DateTime.Today;
        return await _context.CheckIns
            .Include(c => c.Member!) // Nạp Member để lấy FullName
            .Include(c => c.Subscription!)
                .ThenInclude(s => s!.Package) // Nạp Package qua Subscription
            .Where(c => c.CheckInTime >= today && !c.IsDeleted)
            .OrderByDescending(c => c.CheckInTime)
            .ToListAsync();
    }

    public async Task<List<CheckIn>> GetByMemberIdAsync(Guid memberId, int take = 10)
    {
        return await _context.CheckIns
            .Include(c => c.Subscription)
            .Where(c => c.MemberId == memberId)
            .OrderByDescending(c => c.CheckInTime)
            .Take(take)
            .ToListAsync();
    }

    public async Task<CheckIn?> GetActiveCheckInAsync(Guid memberId)
    {
        return await _context.CheckIns
            .Include(c => c.Member)
            .Include(c => c.Subscription)
            .Where(c => c.MemberId == memberId && c.CheckOutTime == null)
            .OrderByDescending(c => c.CheckInTime)
            .FirstOrDefaultAsync();
    }

    public async Task<List<CheckIn>> GetCheckInsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.CheckIns
            .Include(c => c.Member)
            .Include(c => c.Subscription)
            .Where(c => c.CheckInTime >= startDate && c.CheckInTime <= endDate)
            .OrderByDescending(c => c.CheckInTime)
            .ToListAsync();
    }
    public async Task<List<CheckIn>> GetTodayCheckInsWithMemberAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.CheckIns
            .Include(c => c.Member) // <--- QUAN TRỌNG: Nạp thông tin Member
            .Where(c => c.CheckInTime >= today && !c.IsDeleted)
            .OrderByDescending(c => c.CheckInTime)
            .ToListAsync();
    }
}
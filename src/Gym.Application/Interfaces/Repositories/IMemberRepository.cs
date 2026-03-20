using Gym.Application.Interfaces;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByMemberCodeAsync(string memberCode);
    Task<List<Member>> SearchAsync(string keyword);
}

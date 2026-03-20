using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Members;

namespace Gym.Application.Interfaces.Services;

public interface IMemberService
{
    Task<ResponseDto<MemberDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<PaginatedResultDto<MemberListDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<ResponseDto<MemberDto>> CreateAsync(CreateMemberDto dto);
    Task<ResponseDto<MemberDto>> UpdateAsync(Guid id, UpdateMemberDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
    Task<ResponseDto<MemberDto>> GetByMemberCodeAsync(string memberCode);
    Task<ResponseDto<List<MemberListDto>>> SearchAsync(string keyword);
    Task<ResponseDto<MemberDto>> RegisterOnlineAsync(RegisterLeadDto dto);
}
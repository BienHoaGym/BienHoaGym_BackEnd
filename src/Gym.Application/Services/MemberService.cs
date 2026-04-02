using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Members;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Services;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;
    public MemberService(IUnitOfWork unitOfWork, IMapper mapper, IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }


    #region Basic Operations

    public async Task<ResponseDto<MemberDto>> GetByIdAsync(Guid id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);
        if (member == null || member.IsDeleted)
            return ResponseDto<MemberDto>.FailureResult("Không tìm thấy hội viên");

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member));
    }

    public async Task<ResponseDto<PaginatedResultDto<MemberListDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        await ScanAndExpireProspectiveMembersAsync();

        var query = _unitOfWork.Members.GetQueryable()
            .Where(m => !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt);

        var totalCount = await query.CountAsync();
        var members = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var result = new PaginatedResultDto<MemberListDto>
        {
            Items = _mapper.Map<List<MemberListDto>>(members),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return ResponseDto<PaginatedResultDto<MemberListDto>>.SuccessResult(result);
    }

    public async Task<ResponseDto<MemberDto>> CreateAsync(CreateMemberDto dto)
    {
        var emailError = await ValidateEmailUniquenessAsync(dto.Email);
        if (emailError != null) return ResponseDto<MemberDto>.FailureResult(emailError);

        var phoneError = await ValidatePhoneUniquenessAsync(dto.PhoneNumber);
        if (phoneError != null) return ResponseDto<MemberDto>.FailureResult(phoneError);

        var member = _mapper.Map<Member>(dto);
        member.MemberCode = await GenerateUniqueMemberCodeAsync();
        member.Status = MemberStatus.Active;
        member.JoinedDate = DateTime.UtcNow;
        member.UserId = null;

        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member), "Thêm hội viên thành công");
    }

    public async Task<ResponseDto<MemberDto>> UpdateAsync(Guid id, UpdateMemberDto dto)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);
        if (member == null || member.IsDeleted)
            return ResponseDto<MemberDto>.FailureResult("Không tìm thấy hội viên");

        var emailError = await ValidateEmailUniquenessAsync(dto.Email, id);
        if (emailError != null) return ResponseDto<MemberDto>.FailureResult(emailError);

        var phoneError = await ValidatePhoneUniquenessAsync(dto.PhoneNumber, id);
        if (phoneError != null) return ResponseDto<MemberDto>.FailureResult(phoneError);

        _mapper.Map(dto, member);
        member.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member), "Cập nhật thông tin thành công");
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);
        if (member == null || member.IsDeleted)
            return ResponseDto<bool>.FailureResult("Không tìm thấy hội viên");

        member.IsDeleted = true;
        member.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Xóa hội viên thành công");
    }

    #endregion

    #region Specialized Methods

    public async Task<ResponseDto<MemberDto>> GetByMemberCodeAsync(string memberCode)
    {
        var member = await _unitOfWork.Members.GetQueryable()
            .FirstOrDefaultAsync(m => m.MemberCode == memberCode && !m.IsDeleted);

        if (member == null) return ResponseDto<MemberDto>.FailureResult("Không tìm thấy hội viên");

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member));
    }

    public async Task<ResponseDto<List<MemberListDto>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return ResponseDto<List<MemberListDto>>.FailureResult("Vui lòng nhập từ khóa tìm kiếm");

        var members = await _unitOfWork.Members.GetQueryable()
            .Where(m => !m.IsDeleted &&
                       (m.FullName.Contains(keyword) ||
                        m.MemberCode.Contains(keyword) ||
                        (m.Email != null && m.Email.Contains(keyword)) ||
                        m.PhoneNumber.Contains(keyword)))
            .ToListAsync();

        return ResponseDto<List<MemberListDto>>.SuccessResult(_mapper.Map<List<MemberListDto>>(members));
    }

    public async Task<ResponseDto<MemberDto>> RegisterOnlineAsync(RegisterLeadDto dto)
    {
        var existing = await _unitOfWork.Members.GetQueryable()
            .FirstOrDefaultAsync(m => m.PhoneNumber == dto.PhoneNumber && !m.IsDeleted);

        if (existing != null)
            return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(existing), "Thông tin của bạn đã có trong hệ thống");

        var member = new Member
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email ?? string.Empty,
            Note = $"NGUỒN: MARKETING WEB. Quan tâm: {dto.PackageInterest}. Ghi chú: {dto.Notes}",
            Status = MemberStatus.Prospective,
            JoinedDate = DateTime.UtcNow,
            MemberCode = await GenerateUniqueMemberCodeAsync()
        };

        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member), "Đăng ký thành công! Chúng tôi sẽ liên hệ sớm.");
    }

    #endregion

    #region Private Helpers

    private async Task ScanAndExpireProspectiveMembersAsync()
    {
        var expiryLimit = DateTime.UtcNow.AddHours(-24);
        var expiredLeads = await _unitOfWork.Members.GetQueryable()
            .Where(m => m.Status == MemberStatus.Prospective && m.CreatedAt < expiryLimit && !m.IsDeleted)
            .ToListAsync();

        if (expiredLeads.Any())
        {
            foreach (var lead in expiredLeads)
            {
                lead.IsDeleted = true;
                lead.UpdatedAt = DateTime.UtcNow;
                
                await _auditLogService.LogAsync("System", "AUTO_EXPIRE_LEAD", "Members", 
                    new { Status = MemberStatus.Prospective.ToString() }, 
                    new { IsDeleted = true, Reason = "Timeout 24h" });
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private async Task<string?> ValidateEmailUniquenessAsync(string? email, Guid? excludeId = null)
    {
        if (string.IsNullOrEmpty(email)) return null;

        var query = _unitOfWork.Members.GetQueryable()
            .Where(m => m.Email == email && !m.IsDeleted);

        if (excludeId.HasValue)
            query = query.Where(m => m.Id != excludeId.Value);

        if (await query.AnyAsync())
            return excludeId.HasValue ? "Email đã được sử dụng bởi hội viên khác" : "Email đã tồn tại trong hệ thống";

        return null;
    }

    private async Task<string?> ValidatePhoneUniquenessAsync(string? phone, Guid? excludeId = null)
    {
        if (string.IsNullOrEmpty(phone)) return null;

        var query = _unitOfWork.Members.GetQueryable()
            .Where(m => m.PhoneNumber == phone && !m.IsDeleted);

        if (excludeId.HasValue)
            query = query.Where(m => m.Id != excludeId.Value);

        if (await query.AnyAsync())
            return excludeId.HasValue ? "Số điện thoại đã được sử dụng bởi hội viên khác" : "Số điện thoại đã tồn tại trong hệ thống";

        return null;
    }

    private async Task<string> GenerateUniqueMemberCodeAsync()
    {
        // Optimization: Find the latest member code numeric part instead of loading all members
        var lastMember = await _unitOfWork.Members.GetQueryable()
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefaultAsync();

        int nextIndex = 1;
        if (lastMember != null && lastMember.MemberCode.StartsWith("GYM"))
        {
            if (int.TryParse(lastMember.MemberCode.Substring(3), out int lastIndex))
                nextIndex = lastIndex + 1;
        }

        // Safety loop to ensure uniqueness
        while (await _unitOfWork.Members.GetQueryable().AnyAsync(m => m.MemberCode == $"GYM{nextIndex:D4}"))
        {
            nextIndex++;
        }

        return $"GYM{nextIndex:D4}";
    }

    #endregion
}

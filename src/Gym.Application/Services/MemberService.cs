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

    public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<MemberDto>> GetByIdAsync(Guid id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);

        if (member == null || member.IsDeleted)
        {
            return ResponseDto<MemberDto>.FailureResult("Không tìm thấy hội viên");
        }

        var memberDto = _mapper.Map<MemberDto>(member);
        return ResponseDto<MemberDto>.SuccessResult(memberDto);
    }

    public async Task<ResponseDto<PaginatedResultDto<MemberListDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = await _unitOfWork.Members.GetAllAsync();

        var activeMembers = query.Where(m => !m.IsDeleted).OrderByDescending(m => m.CreatedAt);

        var totalCount = activeMembers.Count();

        var members = activeMembers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var memberDtos = _mapper.Map<List<MemberListDto>>(members);

        var result = new PaginatedResultDto<MemberListDto>
        {
            Items = memberDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return ResponseDto<PaginatedResultDto<MemberListDto>>.SuccessResult(result);
    }

    // --- HÀM CREATE ĐÃ ĐƯỢC SỬA LẠI (BỎ dto.UserId) ---
    public async Task<ResponseDto<MemberDto>> CreateAsync(CreateMemberDto dto)
    {
        // 1. Check trùng lặp Email
        if (!string.IsNullOrEmpty(dto.Email))
        {
            var exists = await _unitOfWork.Members.GetQueryable().AnyAsync(m => m.Email == dto.Email && !m.IsDeleted);
            if (exists) return ResponseDto<MemberDto>.FailureResult("Email đã tồn tại trong hệ thống");
        }

        // 2. Check trùng lặp SĐT
        var phoneExists = await _unitOfWork.Members.GetQueryable().AnyAsync(m => m.PhoneNumber == dto.PhoneNumber && !m.IsDeleted);
        if (phoneExists) return ResponseDto<MemberDto>.FailureResult("Số điện thoại đã tồn tại trong hệ thống");

        // 3. Map dữ liệu
        var member = _mapper.Map<Member>(dto);

        // 4. Sinh mã hội viên tự động
        member.MemberCode = await GenerateUniqueMemberCodeAsync();
        member.Status = MemberStatus.Active;
        member.JoinedDate = DateTime.UtcNow;

        // 5. Gán UserId là null (Vì chưa tạo User)
        // Lưu ý: Đảm bảo bạn đã sửa Entity Member cho phép UserId nullable (Guid?)
        member.UserId = null;

        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member), "Thêm hội viên thành công");
    }

    public async Task<ResponseDto<MemberDto>> UpdateAsync(Guid id, UpdateMemberDto dto)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);

        if (member == null || member.IsDeleted)
        {
            return ResponseDto<MemberDto>.FailureResult("Không tìm thấy hội viên");
        }

        // Check trùng Email (trừ chính mình)
        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != member.Email)
        {
            var emailExists = await _unitOfWork.Members.GetQueryable()
                .AnyAsync(m => m.Email == dto.Email && m.Id != id && !m.IsDeleted);
            if (emailExists) return ResponseDto<MemberDto>.FailureResult("Email đã được sử dụng bởi hội viên khác");
        }

        // Check trùng SĐT (trừ chính mình)
        if (!string.IsNullOrEmpty(dto.PhoneNumber) && dto.PhoneNumber != member.PhoneNumber)
        {
            var phoneExists = await _unitOfWork.Members.GetQueryable()
                .AnyAsync(m => m.PhoneNumber == dto.PhoneNumber && m.Id != id && !m.IsDeleted);
            if (phoneExists) return ResponseDto<MemberDto>.FailureResult("Số điện thoại đã được sử dụng bởi hội viên khác");
        }

        // Cập nhật dữ liệu
        member.FullName = dto.FullName;
        member.Email = dto.Email ?? string.Empty;
        member.PhoneNumber = dto.PhoneNumber;
        member.DateOfBirth = dto.DateOfBirth ?? member.DateOfBirth;

        // Các trường bổ sung
        member.Gender = dto.Gender;
        member.Address = dto.Address;
        member.Note = dto.Notes;
        member.EmergencyContact = dto.EmergencyContact;
        member.EmergencyPhone = dto.EmergencyPhone;
        member.FaceEncoding = dto.FaceEncoding;
        member.QRCode = dto.QRCode;

        member.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync();

        var memberDto = _mapper.Map<MemberDto>(member);
        return ResponseDto<MemberDto>.SuccessResult(memberDto, "Cập nhật thông tin thành công");
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);

        if (member == null || member.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Không tìm thấy hội viên");
        }

        member.IsDeleted = true;
        member.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Xóa hội viên thành công");
    }

    public async Task<ResponseDto<MemberDto>> GetByMemberCodeAsync(string memberCode)
    {
        var member = await _unitOfWork.Members.GetQueryable().FirstOrDefaultAsync(m => m.MemberCode == memberCode && !m.IsDeleted);

        if (member == null)
        {
            return ResponseDto<MemberDto>.FailureResult("Không tìm thấy hội viên");
        }

        var memberDto = _mapper.Map<MemberDto>(member);
        return ResponseDto<MemberDto>.SuccessResult(memberDto);
    }

    public async Task<ResponseDto<List<MemberListDto>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return ResponseDto<List<MemberListDto>>.FailureResult("Vui lòng nhập từ khóa tìm kiếm");
        }

        var allMembers = await _unitOfWork.Members.GetAllAsync();

        var members = allMembers
        .Where(m => !m.IsDeleted &&
                   (m.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    m.MemberCode.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(m.Email) && m.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    m.PhoneNumber.Contains(keyword)))
        .ToList();

        var memberDtos = _mapper.Map<List<MemberListDto>>(members);
        return ResponseDto<List<MemberListDto>>.SuccessResult(memberDtos);
    }

    public async Task<ResponseDto<MemberDto>> RegisterOnlineAsync(RegisterLeadDto dto)
    {
        // Kiểm tra xem số điện thoại đã tồn tại chưa để tránh spam
        var existing = await _unitOfWork.Members.GetQueryable()
            .FirstOrDefaultAsync(m => m.PhoneNumber == dto.PhoneNumber && !m.IsDeleted);

        if (existing != null)
        {
            // Nếu đã tồn tại thì trả về thông tin cũ (hoặc báo lỗi tùy logic)
            // Ở đây ta trả về Success để trải nghiệm người dùng mượt mà
            return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(existing), "Thông tin của bạn đã có trong hệ thống");
        }

        var member = new Member
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email ?? string.Empty,
            Note = $"NGUỒN: MARKETING WEB. Quan tâm: {dto.PackageInterest}. Ghi chú: {dto.Notes}",
            Status = MemberStatus.Prospective, // Trạng thái TIỀM NĂNG
            JoinedDate = DateTime.UtcNow,
            MemberCode = await GenerateUniqueMemberCodeAsync()
        };

        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<MemberDto>.SuccessResult(_mapper.Map<MemberDto>(member), "Đăng ký thành công! Chúng tôi sẽ liên hệ sớm.");
    }

    private async Task<string> GenerateUniqueMemberCodeAsync()
    {
        var allMembers = await _unitOfWork.Members.GetAllAsync();
        var existingCodes = allMembers.Select(m => m.MemberCode).ToList();

        int counter = 1;
        string memberCode;

        do
        {
            memberCode = $"GYM{counter:D4}"; // VD: GYM0001
            counter++;
        }
        while (existingCodes.Contains(memberCode));

        return memberCode;
    }
}
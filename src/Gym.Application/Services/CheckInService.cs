using AutoMapper;
using Gym.Application.DTOs.CheckIns;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Members;
using Gym.Application.DTOs.Subscriptions;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym.Application.Services;

public class CheckInService : ICheckInService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService; // ✅ Thêm Audit Log

    public CheckInService(IUnitOfWork unitOfWork, IMapper mapper, IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
    }

    public async Task<ResponseDto<CheckInValidationResultDto>> ValidateCheckInAsync(string memberCode)
    {
        var result = new CheckInValidationResultDto
        {
            Errors = new List<string>(),
            Warnings = new List<string>()
        };

        // 1. Tìm hội viên bằng mã
        var members = await _unitOfWork.Members.FindAsync(m => m.MemberCode == memberCode && !m.IsDeleted);
        var member = members.FirstOrDefault();

        if (member == null)
        {
            result.IsValid = false;
            result.Message = "Không tìm thấy hội viên";
            result.Errors.Add("Mã hội viên không hợp lệ");
            
            // ✅ GHI AUDIT LOG: Thử check-in sai mã
            await _auditLogService.LogAsync("System", "CHECKIN_FAILED", "CheckIns", null, 
                new { MemberCode = memberCode, Reason = "Invalid Member Code" });
                
            return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
        }

        result.Member = _mapper.Map<MemberDto>(member);

        // 2. Kiểm tra trạng thái hội viên
        if (member.Status != MemberStatus.Active)
        {
            result.IsValid = false;
            result.Message = "Hội viên không ở trạng thái hoạt động";
            result.Errors.Add($"Trạng thái hiện tại: {member.Status}");

            // ✅ GHI AUDIT LOG: Check-in khi tài khoản bị khóa/inactive
            await _auditLogService.LogAsync("System", "CHECKIN_FAILED", "CheckIns", null,
                new { MemberCode = memberCode, Status = member.Status.ToString(), Reason = "Account not Active" });

            return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
        }

        // 3. Dùng GetQueryable() kết hợp Include để lấy tên Gói tập (Package)
        var subscriptions = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Package) // Kéo thông tin gói tập lên để không bị N/A
            .Where(s => s.MemberId == member.Id && s.Status == SubscriptionStatus.Active && !s.IsDeleted)
            .ToListAsync();

        var activeSubscription = subscriptions
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();

        if (activeSubscription == null)
        {
            result.IsValid = false;
            result.Message = "Không tìm thấy gói tập đang hoạt động";
            result.Errors.Add("Vui lòng gia hạn gói tập");

            // ✅ GHI AUDIT LOG: Thử check-in khi không có gói tập
            await _auditLogService.LogAsync("System", "CHECKIN_FAILED", "CheckIns", null,
                new { MemberCode = memberCode, Reason = "No Active Subscription found" });

            return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
        }

        result.ActiveSubscription = _mapper.Map<SubscriptionDto>(activeSubscription);

        // 4. Kiểm tra ngày hết hạn
        if (activeSubscription.EndDate < DateTime.Today)
        {
            result.IsValid = false;
            result.Message = "Gói tập đã hết hạn";
            result.Errors.Add($"Ngày hết hạn: {activeSubscription.EndDate:dd/MM/yyyy}");

            // ✅ GHI AUDIT LOG: Check-in khi gói hết hạn
            await _auditLogService.LogAsync("System", "CHECKIN_FAILED", "CheckIns", null,
                new { MemberCode = memberCode, SubscriptionId = activeSubscription.Id, Reason = "Sub Expired" });

            return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
        }

        // 5. Kiểm tra số buổi còn lại
        if (activeSubscription.RemainingSessions.HasValue)
        {
            if (activeSubscription.RemainingSessions <= 0)
            {
                result.IsValid = false;
                result.Message = "Đã hết số buổi tập";
                result.Errors.Add("Vui lòng mua thêm buổi tập");
                return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
            }

            if (activeSubscription.RemainingSessions <= 3)
            {
                result.Warnings.Add($"Chỉ còn {activeSubscription.RemainingSessions} buổi tập");
            }
        }

        // 6. Kiểm tra xem đã Check-in (chưa out) chưa
        var activeCheckIn = await _unitOfWork.CheckIns.GetActiveCheckInAsync(member.Id);

        if (activeCheckIn != null)
        {
            result.IsValid = false;
            result.Message = "Hội viên đang ở trong phòng tập";
            result.Errors.Add($"Đã check-in lúc {activeCheckIn.CheckInTime:HH:mm}");

            // ✅ GHI AUDIT LOG: Cảnh báo dùng chung thẻ
            await _auditLogService.LogAsync("System", "FRAUD_WARNING", "CheckIns", null,
                new { MemberCode = memberCode, Reason = "Double Check-in Attempt" });

            return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
        }

        // 6.5. NGHIỆP VỤ (BA - II.5): Chống gian lận - Giới hạn 1 ngày 1 lần
        var today = DateTime.UtcNow.Date;
        var hasCheckedInToday = await _unitOfWork.CheckIns.GetQueryable()
            .AnyAsync(c => c.MemberId == member.Id
                        && c.CheckInTime.Date == today // Đã tối ưu biến today
                        && !c.IsDeleted);

        if (hasCheckedInToday)
        {
            result.IsValid = false;
            result.Message = "Đã sử dụng hết lượt trong ngày";
            result.Errors.Add("Theo quy định, hội viên chỉ được check-in 1 lần/ngày.");

            // ✅ GHI AUDIT LOG: Cảnh báo Check-in quá giới hạn trong ngày
            await _auditLogService.LogAsync("System", "FRAUD_WARNING", "CheckIns", null,
                new { MemberCode = memberCode, Reason = "Daily Check-in Limit Exceeded" });

            return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
        }

        // 7. Cảnh báo sắp hết hạn
        var daysUntilExpiry = (activeSubscription.EndDate - DateTime.Today).Days;
        if (daysUntilExpiry <= 7)
        {
            result.Warnings.Add($"Gói tập sẽ hết hạn sau {daysUntilExpiry} ngày");
        }

        result.IsValid = true;
        result.Message = "Hợp lệ để check-in";

        return ResponseDto<CheckInValidationResultDto>.SuccessResult(result);
    }

    public async Task<ResponseDto<CheckInDto>> CheckInAsync(CreateCheckInDto dto)
    {
        var validation = await ValidateCheckInAsync(dto.MemberCode);
        if (!validation.Data!.IsValid)
            return ResponseDto<CheckInDto>.FailureResult(validation.Data.Message, validation.Data.Errors);

        return await ExecuteCheckInAsync(validation.Data.Member!.Id, validation.Data.ActiveSubscription!.Id, dto.Notes);
    }

    public async Task<ResponseDto<CheckInDto>> CheckInWithFaceAsync(string faceEncoding)
    {
        // 1. Tìm hội viên khớp với khuôn mặt (Mô phỏng matching chính xác vector)
        var members = await _unitOfWork.Members.FindAsync(m => m.FaceEncoding == faceEncoding && !m.IsDeleted);
        var member = members.FirstOrDefault();

        if (member == null)
            return ResponseDto<CheckInDto>.FailureResult("Không nhận diện được khuôn mặt. Vui lòng thử lại!");

        var validation = await ValidateCheckInAsync(member.MemberCode);
        if (!validation.Data!.IsValid)
            return ResponseDto<CheckInDto>.FailureResult(validation.Data.Message, validation.Data.Errors);

        // ✅ GHI AUDIT LOG: Nhận diện FaceID thành công
        await _auditLogService.LogAsync("System", "FACE_RECOGNITION", "CheckIns", member.Id.ToString(), 
            new { MemberName = member.FullName, Result = "Success" });

        return await ExecuteCheckInAsync(member.Id, validation.Data.ActiveSubscription!.Id, "Check-in qua FaceID");
    }



    private async Task<ResponseDto<CheckInDto>> ExecuteCheckInAsync(Guid memberId, Guid subscriptionId, string? notes)
    {
        var checkIn = new CheckIn
        {
            MemberId = memberId,
            SubscriptionId = subscriptionId,
            CheckInTime = DateTime.UtcNow,
            Note = notes
        };

        await _unitOfWork.CheckIns.AddAsync(checkIn);

        var subscriptionToUpdate = await _unitOfWork.Subscriptions.GetByIdAsync(subscriptionId);
        if (subscriptionToUpdate != null && subscriptionToUpdate.RemainingSessions.HasValue)
        {
            subscriptionToUpdate.RemainingSessions -= 1;
            _unitOfWork.Subscriptions.Update(subscriptionToUpdate);
        }

        await _unitOfWork.SaveChangesAsync();

        var result = await _unitOfWork.CheckIns.GetQueryable()
            .Include(c => c.Member!)
            .Include(c => c.Subscription!).ThenInclude(s => s!.Package)
            .FirstOrDefaultAsync(c => c.Id == checkIn.Id);

        if (result == null) return ResponseDto<CheckInDto>.FailureResult("Lỗi khi tải dữ liệu check-in vừa tạo");

        return ResponseDto<CheckInDto>.SuccessResult(_mapper.Map<CheckInDto>(result));
    }

    public async Task<ResponseDto<List<CheckInDto>>> GetTodayCheckInsAsync()
    {
        var query = _unitOfWork.CheckIns.GetQueryable()
            .Include(c => c.Member!)
            .Include(c => c.Subscription!)
                .ThenInclude(s => s!.Package)
            .Where(c => c.CheckInTime.Date == DateTime.UtcNow.Date && !c.IsDeleted)
            .OrderByDescending(c => c.CheckInTime);

        var checkIns = await query.ToListAsync();
        return ResponseDto<List<CheckInDto>>.SuccessResult(_mapper.Map<List<CheckInDto>>(checkIns));
    }

    public async Task<ResponseDto<bool>> CheckOutAsync(Guid checkInId)
    {
        var checkIn = await _unitOfWork.CheckIns.GetByIdAsync(checkInId);

        if (checkIn == null)
        {
            return ResponseDto<bool>.FailureResult("Không tìm thấy bản ghi check-in");
        }

        if (checkIn.CheckOutTime.HasValue)
        {
            return ResponseDto<bool>.FailureResult("Hội viên đã check-out trước đó");
        }

        checkIn.CheckOutTime = DateTime.UtcNow;
        _unitOfWork.CheckIns.Update(checkIn);
        await _unitOfWork.SaveChangesAsync();

        var duration = checkIn.CheckOutTime.Value - checkIn.CheckInTime;
        return ResponseDto<bool>.SuccessResult(
            true,
            $"Check-out thành công. Thời gian tập: {duration.Hours}h {duration.Minutes}m"
        );
    }

    public async Task<ResponseDto<List<CheckInDto>>> GetMemberHistoryAsync(Guid memberId, int take = 10)
    {
        var query = _unitOfWork.CheckIns.GetQueryable()
            .Include(c => c.Member)
            .Where(c => c.MemberId == memberId && !c.IsDeleted)
            .OrderByDescending(c => c.CheckInTime)
            .Take(take);

        var checkIns = await query.ToListAsync();
        var checkInDtos = _mapper.Map<List<CheckInDto>>(checkIns);

        return ResponseDto<List<CheckInDto>>.SuccessResult(checkInDtos);
    }
}
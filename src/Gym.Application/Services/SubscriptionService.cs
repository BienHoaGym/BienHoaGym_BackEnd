using AutoMapper;
using Gym.Application.DTOs.Common;
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

public class SubscriptionService : ISubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditLogService _auditLogService;
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

    public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper, IAuditLogService auditLogService, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }

    // --- Tự động quét và hủy gói hết hạn / Giao dịch treo ---
    private async Task ScanAndExpireAsync()
    {
        var today = DateTime.UtcNow.Date;
        var now = DateTime.UtcNow;

        // 1. Tìm các gói đang Active nhưng ngày kết thúc đã qua
        var expiredSubs = await _unitOfWork.Subscriptions.GetQueryable()
            .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate < today && !s.IsDeleted)
            .ToListAsync();

        if (expiredSubs.Any())
        {
            foreach (var sub in expiredSubs)
            {
                var oldStatus = sub.Status;
                sub.Status = SubscriptionStatus.Expired;
                sub.UpdatedAt = now;

                await _auditLogService.LogAsync("System", "AUTO_EXPIRE_SUBSCRIPTION", "MemberSubscriptions",
                    new { Status = oldStatus.ToString() },
                    new { Status = sub.Status.ToString() });
            }
        }

        // 2. NGHIỆP VỤ (WF - 2.1): Tự động hủy giao dịch Pending quá 30 phút
        var pendingExpiryLimit = now.AddMinutes(-30);
        var stalledPendingSubs = await _unitOfWork.Subscriptions.GetQueryable()
            .Where(s => s.Status == SubscriptionStatus.Pending && s.CreatedAt < pendingExpiryLimit && !s.IsDeleted)
            .ToListAsync();

        if (stalledPendingSubs.Any())
        {
            foreach (var sub in stalledPendingSubs)
            {
                sub.Status = SubscriptionStatus.Cancelled;
                sub.UpdatedAt = now;
                // sub.Note = "Hủy tự động do quá thời hạn thanh toán (30p)";

                await _auditLogService.LogAsync("System", "AUTO_CANCEL_STALLED_PAYMENT", "MemberSubscriptions",
                    new { Status = SubscriptionStatus.Pending.ToString() },
                    new { Status = sub.Status.ToString(), Reason = "Timeout 30m" });
            }
        }

        if (expiredSubs.Any() || stalledPendingSubs.Any())
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<ResponseDto<SubscriptionDto>> GetByIdAsync(Guid id)
    {
        var subscription = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Member)
            .Include(s => s.Package)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subscription == null || subscription.IsDeleted)
        {
            return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói đăng ký");
        }

        // Cập nhật nóng nếu gói này hết hạn
        if (subscription.Status == SubscriptionStatus.Active && subscription.EndDate < DateTime.UtcNow.Date)
        {
            var oldStatus = subscription.Status;
            subscription.Status = SubscriptionStatus.Expired;
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("System", "AUTO_EXPIRE_SUBSCRIPTION", "MemberSubscriptions",
                new { Status = oldStatus.ToString() },
                new { Status = subscription.Status.ToString() });
        }

        var subscriptionDto = _mapper.Map<SubscriptionDto>(subscription);
        return ResponseDto<SubscriptionDto>.SuccessResult(subscriptionDto);
    }

    public async Task<ResponseDto<SubscriptionDetailDto>> GetDetailAsync(Guid id)
    {
        var subscription = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Member)
            .Include(s => s.Package)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subscription == null || subscription.IsDeleted)
        {
            return ResponseDto<SubscriptionDetailDto>.FailureResult("Không tìm thấy gói đăng ký");
        }

        // Cập nhật nóng
        if (subscription.Status == SubscriptionStatus.Active && subscription.EndDate < DateTime.UtcNow.Date)
        {
            var oldStatus = subscription.Status;
            subscription.Status = SubscriptionStatus.Expired;
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("System", "AUTO_EXPIRE_SUBSCRIPTION", "MemberSubscriptions",
                new { Status = oldStatus.ToString() },
                new { Status = subscription.Status.ToString() });
        }

        var detailDto = _mapper.Map<SubscriptionDetailDto>(subscription);
        return ResponseDto<SubscriptionDetailDto>.SuccessResult(detailDto);
    }

    public async Task<ResponseDto<List<SubscriptionDto>>> GetAllAsync()
    {
        await ScanAndExpireAsync();

        var subscriptions = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Member)
            .Include(s => s.Package)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        var subscriptionDtos = _mapper.Map<List<SubscriptionDto>>(subscriptions);
        return ResponseDto<List<SubscriptionDto>>.SuccessResult(subscriptionDtos);
    }

    public async Task<ResponseDto<List<SubscriptionDto>>> GetByMemberIdAsync(Guid memberId)
    {
        await ScanAndExpireAsync();

        var subscriptions = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Package)
            .Where(s => s.MemberId == memberId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        var subscriptionDtos = _mapper.Map<List<SubscriptionDto>>(subscriptions);
        return ResponseDto<List<SubscriptionDto>>.SuccessResult(subscriptionDtos);
    }

    public async Task<ResponseDto<SubscriptionDto>> CreateAsync(CreateSubscriptionDto dto)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
        if (member == null || member.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy hội viên");

        var package = await _unitOfWork.Packages.GetByIdAsync(dto.PackageId);
        if (package == null || package.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói tập");

        if (!package.IsActive) return ResponseDto<SubscriptionDto>.FailureResult("Gói tập đang tạm khóa");

        // CHẶN: Nếu có gói Active/Pending và chưa hết hạn (Dùng UtcNow.Date để tránh lệch múi giờ)
        var today = DateTime.UtcNow.Date;
        var existingSub = await _unitOfWork.Subscriptions.GetQueryable()
            .Where(s => s.MemberId == dto.MemberId && !s.IsDeleted)
            .Where(s => s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Pending)
            .Where(s => s.EndDate >= today)
            .FirstOrDefaultAsync();

        if (existingSub != null)
        {
            if (existingSub.Status == SubscriptionStatus.Pending)
                return ResponseDto<SubscriptionDto>.FailureResult("Hội viên đang có một giao dịch đăng ký đang chờ thanh toán.");
            return ResponseDto<SubscriptionDto>.FailureResult($"Hội viên đang có gói '{existingSub.OriginalPackageName}' còn hạn đến {existingSub.EndDate:dd/MM/yyyy}. Vui lòng sử dụng tính năng 'Gia hạn' nếu muốn mua nối tiếp.");
        }

        var subscription = _mapper.Map<MemberSubscription>(dto);

        // NGHIỆP VỤ I.2 (STATE MACHINE): Bắt đầu vòng đời luôn là Pending
        subscription.Status = SubscriptionStatus.Pending;
        subscription.RemainingSessions = package.SessionLimit;
        subscription.EndDate = subscription.StartDate.AddDays(package.DurationDays);

        // NGHIỆP VỤ I.3 (SNAPSHOT DATA): Lưu giá chết tại thời điểm mua
        subscription.OriginalPackageName = package.Name;
        subscription.OriginalPrice = package.Price;

        // NGHIỆP VỤ (WF - 2.2): Kiểm soát quyền Lễ tân (Receptionist)
        var userRole = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (userRole == "Receptionist")
        {
            // Lễ tân không được tự ý sửa giá gốc của gói
            if (dto.FinalPrice.HasValue && dto.FinalPrice != package.Price)
            {
                 var discountAmount = package.Price - dto.FinalPrice.Value;
                 var discountPercent = (discountAmount / package.Price) * 100;
                 
                 // Giới hạn giảm giá tối đa 10%
                 if (discountPercent > 10)
                 {
                     return ResponseDto<SubscriptionDto>.FailureResult("Lễ tân chỉ được phép giảm giá tối đa 10%. Vui lòng liên hệ Quản lý.");
                 }
            }
        }

        // Xử lý giảm giá
        subscription.DiscountApplied = package.Price - (dto.FinalPrice ?? package.Price);
        subscription.FinalPrice = dto.FinalPrice ?? package.Price;

        await _unitOfWork.Subscriptions.AddAsync(subscription);
        await _unitOfWork.SaveChangesAsync();

        // --- GHI AUDIT LOG ---
        await _auditLogService.LogAsync(
            userId: dto.UserId ?? "System",
            action: "CREATE_SUBSCRIPTION",
            entityName: "MemberSubscriptions",
            oldValues: null,
            newValues: new
            {
                subscription.Id,
                subscription.MemberId,
                subscription.OriginalPackageName,
                subscription.FinalPrice,
                subscription.Status
            }
        );

        var result = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Member)
            .Include(s => s.Package)
            .FirstOrDefaultAsync(s => s.Id == subscription.Id);

        return ResponseDto<SubscriptionDto>.SuccessResult(
            _mapper.Map<SubscriptionDto>(result), "Tạo đăng ký thành công.");
    }

    public async Task<ResponseDto<bool>> ActivateAsync(Guid id)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);
        if (subscription == null) return ResponseDto<bool>.FailureResult("Không tìm thấy gói đăng ký");

        // NGHIỆP VỤ I.2 (STATE MACHINE): Khóa chiều trạng thái, CHỈ cho phép Pending -> Active
        if (subscription.Status != SubscriptionStatus.Pending)
        {
            return ResponseDto<bool>.FailureResult("Chỉ có thể kích hoạt gói tập đang ở trạng thái chờ thanh toán (Pending).");
        }

        var payments = await _unitOfWork.Payments.GetBySubscriptionIdAsync(id);
        if (!payments.Any(p => p.Status == PaymentStatus.Completed))
            return ResponseDto<bool>.FailureResult("Gói tập chưa được thanh toán thành công.");

        var oldStatus = subscription.Status;
        subscription.Status = SubscriptionStatus.Active;
        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);
        await _unitOfWork.SaveChangesAsync();

        // --- GHI AUDIT LOG ---
        await _auditLogService.LogAsync(
            userId: "System", // TODO: Lấy UserID từ IHttpContextAccessor nếu cần
            action: "ACTIVATE_SUBSCRIPTION",
            entityName: "MemberSubscriptions",
            oldValues: new { Status = oldStatus.ToString() },
            newValues: new { Status = subscription.Status.ToString() }
        );

        return ResponseDto<bool>.SuccessResult(true, "Kích hoạt gói tập thành công.");
    }

    public async Task<ResponseDto<bool>> CancelAsync(Guid id, string reason)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);
        if (subscription == null) return ResponseDto<bool>.FailureResult("Không tìm thấy gói đăng ký");

        // NGHIỆP VỤ I.2 (STATE MACHINE): Chặn thao tác hủy bất hợp lý
        if (subscription.Status == SubscriptionStatus.Cancelled)
            return ResponseDto<bool>.FailureResult("Gói tập này đã bị hủy trước đó.");

        if (subscription.Status == SubscriptionStatus.Expired)
            return ResponseDto<bool>.FailureResult("Không thể hủy gói tập đã hết hạn.");

        var oldStatus = subscription.Status;
        subscription.Status = SubscriptionStatus.Cancelled;
        // Nếu có thuộc tính Note thì gán vào đây
        // subscription.CancelReason = reason;

        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);
        await _unitOfWork.SaveChangesAsync();

        // --- GHI AUDIT LOG ---
        await _auditLogService.LogAsync(
            userId: "System", // TODO: Lấy UserID từ HttpContext nếu cần
            action: "CANCEL_SUBSCRIPTION",
            entityName: "MemberSubscriptions",
            oldValues: new { Status = oldStatus.ToString(), Reason = "" },
            newValues: new { Status = subscription.Status.ToString(), Reason = reason }
        );

        return ResponseDto<bool>.SuccessResult(true, "Đã hủy gói tập thành công.");
    }

    public async Task<ResponseDto<List<SubscriptionDto>>> GetExpiringAsync(int days = 7)
    {
        await ScanAndExpireAsync();

        var expiryDate = DateTime.Today.AddDays(days);
        var subscriptions = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Member)
            .Include(s => s.Package)
            .Where(s => s.Status == SubscriptionStatus.Active &&
                        s.EndDate <= expiryDate &&
                        s.EndDate >= DateTime.Today &&
                        !s.IsDeleted)
            .ToListAsync();

        var dtos = _mapper.Map<List<SubscriptionDto>>(subscriptions);
        return ResponseDto<List<SubscriptionDto>>.SuccessResult(dtos, $"Tìm thấy {dtos.Count} gói sắp hết hạn.");
    }

    public async Task<ResponseDto<SubscriptionDto>> RenewAsync(Guid id, Guid packageId)
    {
        var currentSub = await _unitOfWork.Subscriptions.GetByIdAsync(id);
        if (currentSub == null || currentSub.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói tập hiện tại");

        var package = await _unitOfWork.Packages.GetByIdAsync(packageId);
        if (package == null || package.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói tập mới");

        // Ngày bắt đầu gói mới là ngày sau ngày kết thúc gói cũ, hoặc hôm nay nếu gói cũ đã hết hạn
        var today = DateTime.UtcNow.Date;
        var startDate = currentSub.EndDate > today ? currentSub.EndDate.AddDays(1) : today;

        var newSubscription = new MemberSubscription
        {
            MemberId = currentSub.MemberId,
            PackageId = packageId,
            StartDate = startDate,
            EndDate = startDate.AddDays(package.DurationDays),
            Status = SubscriptionStatus.Pending,
            RemainingSessions = package.SessionLimit,
            OriginalPackageName = package.Name,
            OriginalPrice = package.Price,
            DiscountApplied = 0,
            FinalPrice = package.Price
        };

        await _unitOfWork.Subscriptions.AddAsync(newSubscription);
        await _unitOfWork.SaveChangesAsync();

        var result = await _unitOfWork.Subscriptions.GetQueryable()
            .Include(s => s.Member)
            .Include(s => s.Package)
            .FirstOrDefaultAsync(s => s.Id == newSubscription.Id);

        return ResponseDto<SubscriptionDto>.SuccessResult(_mapper.Map<SubscriptionDto>(result), "Gia hạn gói tập thành công (chờ thanh toán)");
    }

    public async Task<ResponseDto<bool>> PauseAsync(Guid id)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);
        if (subscription == null || subscription.IsDeleted) return ResponseDto<bool>.FailureResult("Không tìm thấy gói tập");

        if (subscription.Status != SubscriptionStatus.Active)
            return ResponseDto<bool>.FailureResult("Chỉ có thể tạm dừng gói tập đang hoạt động");

        var oldStatus = subscription.Status;
        subscription.Status = SubscriptionStatus.Suspended;
        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("System", "PAUSE_SUBSCRIPTION", "MemberSubscriptions",
            new { Status = oldStatus.ToString() },
            new { Status = subscription.Status.ToString() });

        return ResponseDto<bool>.SuccessResult(true, "Đã tạm dừng gói tập thành công");
    }
}

﻿﻿using AutoMapper;

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

    // --- Tá»± Ä‘á»™ng quÃ©t và hủy gÃ³i hết háº¡n / Giao dá»‹ch treo ---

    private async Task ScanAndExpireAsync()

    {

        var today = DateTime.UtcNow.Date;

        var now = DateTime.UtcNow;

        // 1. TÃ¬m cÃ¡c gÃ³i Ä‘ang Active nhÆ°ng ngày kết thúc Ä‘Ã£ qua

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

        // 2. NGHIá»†P Vá»¤ (WF - 2.1): Tá»± Ä‘á»™ng hủy giao dá»‹ch Pending quÃ¡ 30 phút

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

                // sub.Note = "Há»§y tá»± Ä‘á»™ng do quÃ¡ thá»i háº¡n thanh toán (30p)";

                await _auditLogService.LogAsync("System", "AUTO_CANCEL_STALLED_PAYMENT", "MemberSubscriptions",

                    new { Status = SubscriptionStatus.Pending.ToString() },

                    new { Status = sub.Status.ToString(), Reason = "Timeout 30m" });

            }

        }

                        // Đồng bộ trạng thái cho TẤT CẢ các hội viên để đảm bảo dữ liệu hiển thị chính xác
        var allMembers = await _unitOfWork.Members.GetQueryable().Where(m => !m.IsDeleted).ToListAsync();
        foreach (var member in allMembers)
        {
            var subs = await _unitOfWork.Subscriptions.GetQueryable()
                .Where(s => s.MemberId == member.Id && !s.IsDeleted)
                .ToListAsync();

            if (!subs.Any())
            {
                if (member.Status != MemberStatus.Prospective && member.Status != MemberStatus.Inactive)
                {
                    member.Status = MemberStatus.Inactive;
                    _unitOfWork.Members.Update(member);
                }
                continue;
            }

            var oldStatus = member.Status;
            if (subs.Any(s => s.Status == SubscriptionStatus.Active))
                member.Status = MemberStatus.Active;
            else if (subs.Any(s => s.Status == SubscriptionStatus.Suspended))
                member.Status = MemberStatus.Suspended;
            else if (subs.Any(s => s.Status == SubscriptionStatus.Pending))
                member.Status = member.Status == MemberStatus.Active ? MemberStatus.Active : MemberStatus.Inactive;
            else
                member.Status = MemberStatus.Inactive;

            if (oldStatus != member.Status)
            {
                _unitOfWork.Members.Update(member);
            }
        }
        await _unitOfWork.SaveChangesAsync();

    }

    public async Task<ResponseDto<SubscriptionDto>> GetByIdAsync(Guid id)

    {

        var subscription = await _unitOfWork.Subscriptions.GetQueryable()

            .Include(s => s.Member)

            .Include(s => s.Package)

            .FirstOrDefaultAsync(s => s.Id == id);

        if (subscription == null || subscription.IsDeleted)

        {

            return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gÃ³i Ä‘Äƒng kÃ½");

        }

        // Cáº­p nháº­t nÃ³ng náº¿u gÃ³i này hết háº¡n

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

            return ResponseDto<SubscriptionDetailDto>.FailureResult("Không tìm thấy gÃ³i Ä‘Äƒng kÃ½");

        }

        // Cáº­p nháº­t nÃ³ng

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

        if (member == null || member.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy há»™i viÃªn");

        var package = await _unitOfWork.Packages.GetByIdAsync(dto.PackageId);

        if (package == null || package.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói tập");

        if (!package.IsActive) return ResponseDto<SubscriptionDto>.FailureResult("Gói tập Ä‘ang táº¡m khÃ³a");

        // CHáº¶N: Náº¿u cÃ³ gÃ³i Active/Pending/Suspended

        var today = DateTime.UtcNow.Date;

        var existingSub = await _unitOfWork.Subscriptions.GetQueryable()

            .Where(s => s.MemberId == dto.MemberId && !s.IsDeleted)

            .Where(s => s.Status == SubscriptionStatus.Active || 

                       s.Status == SubscriptionStatus.Pending || 

                       s.Status == SubscriptionStatus.Suspended)

            .OrderByDescending(s => s.EndDate)

            .FirstOrDefaultAsync();

        if (existingSub != null)

        {

            if (existingSub.Status == SubscriptionStatus.Suspended)

                return ResponseDto<SubscriptionDto>.FailureResult("Há»™i viÃªn Ä‘ang bá»‹ tạm dừng gói tập. Vui lÃ²ng kích hoạt lại gÃ³i cÅ© trÆ°á»›c khi mua gÃ³i mới.");

            if (existingSub.Status == SubscriptionStatus.Pending)

                return ResponseDto<SubscriptionDto>.FailureResult("Há»™i viÃªn Ä‘ang cÃ³ má»™t giao dá»‹ch Ä‘Äƒng kÃ½ Ä‘ang chá» thanh toán.");

            // Kiá»ƒm tra thá»i háº¡n cÃ²n láº¡i

            if (existingSub.Status == SubscriptionStatus.Active && existingSub.EndDate >= today)

            {

                var daysLeft = (existingSub.EndDate - today).Days;

                if (daysLeft > 1)

                {

                    return ResponseDto<SubscriptionDto>.FailureResult($"Gói tập hiện tại ('{existingSub.OriginalPackageName}') vẫn còn hạn Ä‘áº¿n {existingSub.EndDate:dd/MM/yyyy} ({daysLeft} ngày). Chỉ Ä‘Æ°á»£c gia hạn khi gói sắp hết hạn trong vòng 1 ngày.");

                }

            }

        }

        var subscription = _mapper.Map<MemberSubscription>(dto);

        // NGHIá»†P Vá»¤ I.2 (STATE MACHINE): Báº¯t Ä‘áº§u vòng Ä‘á»i luÃ´n là Pending

        subscription.Status = SubscriptionStatus.Pending;

        subscription.RemainingSessions = package.SessionLimit;

        subscription.EndDate = subscription.StartDate.AddDays(package.DurationDays);

        // NGHIá»†P Vá»¤ I.3 (SNAPSHOT DATA): LÆ°u giÃ¡ chết táº¡i thá»i Ä‘iá»ƒm mua

        subscription.OriginalPackageName = package.Name;

        subscription.OriginalPrice = package.Price;

        // NGHIá»†P Vá»¤ (WF - 2.2): Kiá»ƒm soÃ¡t quyá»n Lá»… tÃ¢n (Receptionist)

        var userRole = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        if (userRole == "Receptionist")

        {

            // Lá»… tÃ¢n khÃ´ng Ä‘Æ°á»£c tá»± Ã½ sá»­a giÃ¡ gá»‘c cá»§a gÃ³i

            if (dto.FinalPrice.HasValue && dto.FinalPrice != package.Price)

            {

                 var discountAmount = package.Price - dto.FinalPrice.Value;

                 var discountPercent = (discountAmount / package.Price) * 100;

                 // Giá»›i háº¡n giáº£m giÃ¡ tá»‘i Ä‘a 10%

                 if (discountPercent > 10)

                 {

                     return ResponseDto<SubscriptionDto>.FailureResult("Lá»… tÃ¢n chỉ Ä‘Æ°á»£c phép giáº£m giÃ¡ tá»‘i Ä‘a 10%. Vui lÃ²ng liÃªn hệ Quáº£n lÃ½.");

                 }

            }

        }

        // Xá»­ lÃ½ giáº£m giÃ¡

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

        if (subscription == null) return ResponseDto<bool>.FailureResult("Không tìm thấy gÃ³i Ä‘Äƒng kÃ½");

        // NGHIá»†P Vá»¤ I.2 (STATE MACHINE): KhÃ³a chiá»u tráº¡ng thÃ¡i, CHá»ˆ cho phép Pending -> Active

        if (subscription.Status != SubscriptionStatus.Pending)

        {

            return ResponseDto<bool>.FailureResult("Chỉ có thể kÃ­ch hoáº¡t gói tập đang ở trạng thái chá» thanh toán (Pending).");

        }

        var payments = await _unitOfWork.Payments.GetBySubscriptionIdAsync(id);

        if (!payments.Any(p => p.Status == PaymentStatus.Completed))

            return ResponseDto<bool>.FailureResult("Gói tập chÆ°a Ä‘Æ°á»£c thanh toán thành công.");

                var oldStatus = subscription.Status;

        // LOGIC BAO LUU THOI GIAN: Cong so ngay nghi vao EndDate

        if (subscription.LastPausedAt.HasValue)
        {
            var actualPauseDuration = DateTime.UtcNow - subscription.LastPausedAt.Value;
            var actualDays = (int)Math.Ceiling(actualPauseDuration.TotalDays);

            if (subscription.AutoPauseExtensionDays.HasValue && subscription.AutoPauseExtensionDays.Value > 0)
            {
                // Điều chỉnh: Ngày hết hạn thực tế = Ngày hết hạn cũ + Số ngày nghỉ thực tế
                // Vì ngày hiện tại đã được cộng EstimatedDays, nên ta cộng chênh lệch (actual - estimated)
                var adjustmentDays = actualDays - subscription.AutoPauseExtensionDays.Value;
                subscription.EndDate = subscription.EndDate.AddDays(adjustmentDays);
            }
            else
            {
                // Trường hợp nghỉ tự do, cộng dồn toàn bộ số ngày nghỉ thực tế
                subscription.EndDate = subscription.EndDate.AddDays(actualDays);
            }
        }

        subscription.Status = SubscriptionStatus.Active;

        subscription.LastPausedAt = null;
        subscription.AutoPauseExtensionDays = null;

        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);

        // --- Äá»’NG Bá»˜ TRáº NG THÃI Há»˜I VIÃŠN ---

        var member = await _unitOfWork.Members.GetByIdAsync(subscription.MemberId);

        if (member != null && member.Status != MemberStatus.Active)

        {

            member.Status = MemberStatus.Active;

            _unitOfWork.Members.Update(member);

        }

        await _unitOfWork.SaveChangesAsync();

        // --- GHI AUDIT LOG ---

        await _auditLogService.LogAsync(

            userId: "System", // TODO: Láº¥y UserID tá»« IHttpContextAccessor náº¿u cáº§n

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

        if (subscription == null) return ResponseDto<bool>.FailureResult("Không tìm thấy gÃ³i Ä‘Äƒng kÃ½");

        // NGHIá»†P Vá»¤ I.2 (STATE MACHINE): Cháº·n thao tÃ¡c hủy báº¥t há»£p lÃ½

        if (subscription.Status == SubscriptionStatus.Cancelled)

            return ResponseDto<bool>.FailureResult("Gói tập này Ä‘Ã£ bá»‹ hủy trÆ°á»›c Ä‘Ã³.");

        if (subscription.Status == SubscriptionStatus.Expired)

            return ResponseDto<bool>.FailureResult("KhÃ´ng thá»ƒ hủy gói tập đã hết hạn.");

        var oldStatus = subscription.Status;

        subscription.Status = SubscriptionStatus.Cancelled;

        // Náº¿u cÃ³ thuá»™c tính Note thÃ¬ gán vào Ä‘Ã¢y

        // subscription.CancelReason = reason;

        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);

        await _unitOfWork.SaveChangesAsync();

        // --- GHI AUDIT LOG ---

        await _auditLogService.LogAsync(

            userId: "System", // TODO: Láº¥y UserID tá»« HttpContext náº¿u cáº§n

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

            // CHáº¶N: Náº¿u há»™i viÃªn Ä‘Ã£ gia hạn (cÃ³ gÃ³i khÃ¡c kết thúc muá»™n hÆ¡n) thÃ¬ khÃ´ng coi là sắp hết hạn

            .Where(s => !_unitOfWork.Subscriptions.GetQueryable()

                .Any(futureSub => futureSub.MemberId == s.MemberId && 

                                 !futureSub.IsDeleted && 

                                 (futureSub.Status == SubscriptionStatus.Active || futureSub.Status == SubscriptionStatus.Pending) && 

                                 futureSub.EndDate > s.EndDate))

            .ToListAsync();

        var dtos = _mapper.Map<List<SubscriptionDto>>(subscriptions);

        return ResponseDto<List<SubscriptionDto>>.SuccessResult(dtos, $"Tìm thấy {dtos.Count} gói sắp hết hạn.");

    }

    public async Task<ResponseDto<SubscriptionDto>> RenewAsync(Guid id, Guid packageId)

    {

        var currentSub = await _unitOfWork.Subscriptions.GetByIdAsync(id);

        if (currentSub == null || currentSub.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói tập hiện tại");

        // NGHIá»†P Vá»¤: Chỉ cho phép gia hạn náº¿u gÃ³i cÅ© sắp hết hạn (1 ngày)

        var today = DateTime.UtcNow.Date;

        if (currentSub.Status == SubscriptionStatus.Active && currentSub.EndDate >= today)

        {

            var daysLeft = (currentSub.EndDate - today).Days;

            if (daysLeft > 1)

            {

                return ResponseDto<SubscriptionDto>.FailureResult($"Gói tập hiện tại vẫn còn hạn {daysLeft} ngày. Hệ thống chỉ cho phép gia hạn khi gói sắp hết hạn trong vòng 1 ngày.");

            }

        }

        var package = await _unitOfWork.Packages.GetByIdAsync(packageId);

        if (package == null || package.IsDeleted) return ResponseDto<SubscriptionDto>.FailureResult("Không tìm thấy gói tập mới");

        // Ngày bắt đầu gÃ³i mới là ngày sau ngày kết thúc gÃ³i cÅ©, hoáº·c hÃ´m nay náº¿u gÃ³i cÅ© đã hết hạn

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

    public async Task<ResponseDto<bool>> PauseAsync(Guid id, int? durationDays = null)

    {

        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);

        if (subscription == null || subscription.IsDeleted) return ResponseDto<bool>.FailureResult("Không tìm thấy gói tập");

        if (subscription.Status != SubscriptionStatus.Active)

            return ResponseDto<bool>.FailureResult("Chỉ có thể tạm dừng gói tập đang hoạt động");

                var oldStatus = subscription.Status;

        subscription.Status = SubscriptionStatus.Suspended;

                if (durationDays.HasValue && durationDays.Value > 0)
        {
            // TRƯỜNG HỢP 1: Tạm nghỉ có thời hạn xác định
            // Ngay lập tức cộng dồn ngày dự kiến
            subscription.EndDate = subscription.EndDate.AddDays(durationDays.Value);
            subscription.LastPausedAt = DateTime.UtcNow; // Vẫn phải lưu ngày bắt đầu nghỉ để tính toán thực tế sau này
            subscription.AutoPauseExtensionDays = durationDays.Value; // Lưu lại số ngày đã gia hạn tạm tính
        }
        else
        {
            // TRƯỜNG HỢP 2: Tạm nghỉ không xác định ngày về
            subscription.LastPausedAt = DateTime.UtcNow;
            subscription.AutoPauseExtensionDays = null;
        }

        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("System", "PAUSE_SUBSCRIPTION", "MemberSubscriptions",

            new { Status = oldStatus.ToString() },

            new { Status = subscription.Status.ToString() });

        return ResponseDto<bool>.SuccessResult(true, "Đã tạm dừng gói tập thành công");

    }

    public async Task<ResponseDto<bool>> ResumeAsync(Guid id)

    {

        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);

        if (subscription == null || subscription.IsDeleted) return ResponseDto<bool>.FailureResult("Không tìm thấy gói tập");

        if (subscription.Status != SubscriptionStatus.Suspended)

            return ResponseDto<bool>.FailureResult("Chỉ có thể kích hoạt lại gói tập đang ở trạng thái tạm dừng");

                var oldStatus = subscription.Status;

        // LOGIC BAO LUU THOI GIAN: Cong so ngay nghi vao EndDate

        if (subscription.LastPausedAt.HasValue)
        {
            var actualPauseDuration = DateTime.UtcNow - subscription.LastPausedAt.Value;
            var actualDays = (int)Math.Ceiling(actualPauseDuration.TotalDays);

            if (subscription.AutoPauseExtensionDays.HasValue && subscription.AutoPauseExtensionDays.Value > 0)
            {
                // Điều chỉnh: Ngày hết hạn thực tế = Ngày hết hạn cũ + Số ngày nghỉ thực tế
                // Vì ngày hiện tại đã được cộng EstimatedDays, nên ta cộng chênh lệch (actual - estimated)
                var adjustmentDays = actualDays - subscription.AutoPauseExtensionDays.Value;
                subscription.EndDate = subscription.EndDate.AddDays(adjustmentDays);
            }
            else
            {
                // Trường hợp nghỉ tự do, cộng dồn toàn bộ số ngày nghỉ thực tế
                subscription.EndDate = subscription.EndDate.AddDays(actualDays);
            }
        }

        subscription.Status = SubscriptionStatus.Active;

        subscription.LastPausedAt = null;
        subscription.AutoPauseExtensionDays = null;

        subscription.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Subscriptions.Update(subscription);

        // --- Äá»’NG Bá»˜ TRáº NG THÃI Há»˜I VIÃŠN ---

        var member = await _unitOfWork.Members.GetByIdAsync(subscription.MemberId);

        if (member != null && member.Status != MemberStatus.Active)

        {

            member.Status = MemberStatus.Active;

            _unitOfWork.Members.Update(member);

        }

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync("System", "RESUME_SUBSCRIPTION", "MemberSubscriptions",

            new { Status = oldStatus.ToString() },

            new { Status = subscription.Status.ToString() });

        return ResponseDto<bool>.SuccessResult(true, "Kích hoạt lại gói tập thành công");

    }


    public async Task ProcessStatusScanAsync() { await ScanAndExpireAsync(); }
}
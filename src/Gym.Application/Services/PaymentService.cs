using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Payments;
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

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<PaymentDto>> GetByIdAsync(Guid id)
    {
        var payment = await _unitOfWork.Payments.GetQueryable()
            .Include(p => p.Subscription!).ThenInclude(s => s!.Member)
            .Include(p => p.Subscription!).ThenInclude(s => s!.Package)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null || payment.IsDeleted)
        {
            return ResponseDto<PaymentDto>.FailureResult("Không tìm thấy giao dịch");
        }

        var paymentDto = _mapper.Map<PaymentDto>(payment);
        return ResponseDto<PaymentDto>.SuccessResult(paymentDto);
    }

    public async Task<ResponseDto<List<PaymentDto>>> GetAllAsync()
    {
        var payments = await _unitOfWork.Payments.GetQueryable()
            .Include(p => p.Subscription!).ThenInclude(s => s!.Member)
            .Include(p => p.Subscription!).ThenInclude(s => s!.Package)
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        var paymentDtos = _mapper.Map<List<PaymentDto>>(payments);
        return ResponseDto<List<PaymentDto>>.SuccessResult(paymentDtos);
    }

    public async Task<ResponseDto<List<PaymentDto>>> GetBySubscriptionIdAsync(Guid subscriptionId)
    {
        var payments = await _unitOfWork.Payments.GetQueryable()
            .Include(p => p.Subscription!).ThenInclude(s => s!.Member)
            .Include(p => p.Subscription!).ThenInclude(s => s!.Package)
            .Where(p => p.MemberSubscriptionId == subscriptionId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        var paymentDtos = _mapper.Map<List<PaymentDto>>(payments);
        return ResponseDto<List<PaymentDto>>.SuccessResult(paymentDtos);
    }

    public async Task<ResponseDto<PaymentDto>> ProcessPaymentAsync(ProcessPaymentDto dto)
    {
        // 1. Kiểm tra gói đăng ký tồn tại
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(dto.MemberSubscriptionId);
        if (subscription == null || subscription.IsDeleted)
        {
            return ResponseDto<PaymentDto>.FailureResult("Không tìm thấy gói đăng ký");
        }

        // --- BỔ SUNG LOGIC CHẶN NGHIỆP VỤ TẠI ĐÂY ---
        // NGHIỆP VỤ BA (I.5): Không được thanh toán nếu tổng tiền âm hoặc sai lệch với gói đăng ký
        if (dto.Amount < 0)
        {
            return ResponseDto<PaymentDto>.FailureResult("Số tiền thanh toán không được là số âm.");
        }

        // Đảm bảo số tiền thanh toán không vượt quá số tiền cần thanh toán của gói tập
        // Giả định bảng Subscription có lưu FinalPrice (như đã thêm ở bài trước)
        // Nếu không có FinalPrice, bạn có thể so sánh với OriginalPrice hoặc Package.Price
        /* if (dto.Amount > subscription.FinalPrice)
        {
             return ResponseDto<PaymentDto>.FailureResult("Số tiền thanh toán lớn hơn giá trị gói tập.");
        }
        */

        // 2. Chặn hội viên có quá 1 gói Active
        // Nếu gói này chuẩn bị được kích hoạt (từ Pending -> Active)
        if (subscription.Status == SubscriptionStatus.Pending)
        {
            // Kiểm tra xem Hội viên có gói nào ĐANG ACTIVE khác không?
            var activeSub = await _unitOfWork.Subscriptions.FindAsync(s =>
                s.MemberId == subscription.MemberId &&
                s.Status == SubscriptionStatus.Active &&
                s.Id != subscription.Id && // Không tính chính nó
                !s.IsDeleted
            );

            if (activeSub.Any())
            {
                // Nếu có, CHẶN THANH TOÁN và báo lỗi
                return ResponseDto<PaymentDto>.FailureResult(
                    "Thanh toán thất bại: Hội viên này đang có một gói tập khác đang hoạt động. " +
                    "Vui lòng chờ gói cũ hết hạn hoặc hủy gói cũ trước khi thanh toán gói mới."
                );
            }
        }
        // ---------------------------------------------

        var payment = _mapper.Map<Payment>(dto);
        payment.PaymentDate = DateTime.UtcNow;
        payment.Status = PaymentStatus.Completed;

        await _unitOfWork.Payments.AddAsync(payment);

        // Tự động kích hoạt gói tập nếu thanh toán thành công
        if (payment.Status == PaymentStatus.Completed && subscription.Status == SubscriptionStatus.Pending)
        {
            subscription.Status = SubscriptionStatus.Active;
            subscription.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Subscriptions.Update(subscription);
        }

        await _unitOfWork.SaveChangesAsync();

        var savedPayment = await _unitOfWork.Payments.GetQueryable()
            .Include(p => p.Subscription!).ThenInclude(s => s!.Member)
            .Include(p => p.Subscription!).ThenInclude(s => s!.Package)
            .FirstOrDefaultAsync(p => p.Id == payment.Id);

        if (savedPayment == null) return ResponseDto<PaymentDto>.FailureResult("Lỗi khi tải dữ liệu thanh toán");

        var paymentDto = _mapper.Map<PaymentDto>(savedPayment);
        return ResponseDto<PaymentDto>.SuccessResult(paymentDto, "Thanh toán thành công. Gói tập đã được kích hoạt.");
    }

    public async Task<ResponseDto<bool>> RefundAsync(Guid id, string reason)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);

        if (payment == null || payment.IsDeleted)
            return ResponseDto<bool>.FailureResult("Không tìm thấy giao dịch");

        // NGHIỆP VỤ BA (I.5): Không được hoàn tiền 2 lần
        if (payment.Status == PaymentStatus.Refunded)
            return ResponseDto<bool>.FailureResult("Giao dịch này đã được hoàn tiền trước đó. Không thể hoàn tiền 2 lần.");

        if (payment.Status != PaymentStatus.Completed)
            return ResponseDto<bool>.FailureResult("Chỉ có thể hoàn tiền cho giao dịch đã hoàn tất");

        // NGHIỆP VỤ BA (I.2): STATE MACHINE
        payment.Status = PaymentStatus.Refunded;
        payment.Note = $"Hoàn tiền: {reason}. Ghi chú cũ: {payment.Note}";
        payment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Payments.Update(payment);

        // Hủy gói tập liên quan
        var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(payment.MemberSubscriptionId);
        if (subscription != null)
        {
            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Subscriptions.Update(subscription);
        }

        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, $"Đã hoàn tiền. Lý do: {reason}");
    }
}

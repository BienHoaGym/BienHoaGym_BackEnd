using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Payments;

namespace Gym.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<ResponseDto<PaymentDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<List<PaymentDto>>> GetAllAsync();
    Task<ResponseDto<List<PaymentDto>>> GetBySubscriptionIdAsync(Guid subscriptionId);
    Task<ResponseDto<PaymentDto>> ProcessPaymentAsync(ProcessPaymentDto dto);
    Task<ResponseDto<bool>> RefundAsync(Guid id, string reason);
}

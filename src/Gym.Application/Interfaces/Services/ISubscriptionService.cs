using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Subscriptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces.Services;

public interface ISubscriptionService
{
    Task<ResponseDto<SubscriptionDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<SubscriptionDetailDto>> GetDetailAsync(Guid id);
    Task<ResponseDto<List<SubscriptionDto>>> GetAllAsync();
    Task<ResponseDto<List<SubscriptionDto>>> GetByMemberIdAsync(Guid memberId);
    Task<ResponseDto<SubscriptionDto>> CreateAsync(CreateSubscriptionDto dto);
    Task<ResponseDto<bool>> ActivateAsync(Guid id);
    Task<ResponseDto<bool>> CancelAsync(Guid id, string reason);
    Task<ResponseDto<List<SubscriptionDto>>> GetExpiringAsync(int days = 7);
    Task<ResponseDto<SubscriptionDto>> RenewAsync(Guid id, Guid packageId);
    Task<ResponseDto<bool>> PauseAsync(Guid id, int? durationDays = null);
    Task<ResponseDto<bool>> ResumeAsync(Guid id);
    Task ProcessStatusScanAsync();
}

using Gym.Application.DTOs.CheckIns;
using Gym.Application.DTOs.Common;

namespace Gym.Application.Interfaces.Services;

public interface ICheckInService
{
    Task<ResponseDto<CheckInValidationResultDto>> ValidateCheckInAsync(string memberCode);
    Task<ResponseDto<CheckInDto>> CheckInAsync(CreateCheckInDto dto);
    Task<ResponseDto<CheckInDto>> CheckInWithFaceAsync(string? faceEncoding);
    Task<ResponseDto<CheckInDto>> CheckInWithQRCodeAsync(string qrCode);

    Task<ResponseDto<bool>> CheckOutAsync(Guid checkInId);
    Task<ResponseDto<List<CheckInDto>>> GetTodayCheckInsAsync();
    Task<ResponseDto<List<CheckInDto>>> GetMemberHistoryAsync(Guid memberId, int take = 10);
}

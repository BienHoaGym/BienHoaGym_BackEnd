using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Classes;

namespace Gym.Application.Interfaces.Services;

public interface IClassService
{
    Task<ResponseDto<ClassDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<List<ClassDto>>> GetAllAsync();
    Task<ResponseDto<ClassDto>> CreateAsync(CreateClassDto dto);
    Task<ResponseDto<ClassDto>> UpdateAsync(Guid id, UpdateClassDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
    Task<ResponseDto<List<ClassDto>>> GetActiveClassesAsync();
    Task<ResponseDto<bool>> EnrollMemberAsync(Guid classId, EnrollClassDto dto);
    Task<ResponseDto<bool>> UnenrollMemberAsync(Guid classId, Guid memberId);
    
    // Attendance
    Task<ResponseDto<List<ClassAttendanceDto>>> GetEnrollmentsAsync(Guid classId);
    Task<ResponseDto<bool>> MarkAttendanceAsync(MarkAttendanceDto dto);
}
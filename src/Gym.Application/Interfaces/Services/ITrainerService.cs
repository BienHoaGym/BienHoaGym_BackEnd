using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Trainers;

namespace Gym.Application.Interfaces.Services;

public interface ITrainerService
{
    Task<ResponseDto<TrainerDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<List<TrainerDto>>> GetAllAsync();
    Task<ResponseDto<List<TrainerDto>>> GetAvailableAsync();
    Task<ResponseDto<TrainerDto>> CreateAsync(CreateTrainerDto dto);
    Task<ResponseDto<TrainerDto>> UpdateAsync(Guid id, UpdateTrainerDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);

    // Student Management
    Task<ResponseDto<List<TrainerAssignmentDto>>> GetAssignedMembersAsync(Guid trainerId);
    Task<ResponseDto<TrainerAssignmentDto>> AssignMemberAsync(CreateTrainerAssignmentDto dto);
    Task<ResponseDto<bool>> RemoveAssignmentAsync(Guid assignmentId);
    Task<ResponseDto<PersonalScheduleDto>> GetPersonalScheduleAsync(Guid userId);
    Task<ResponseDto<PersonalScheduleDto>> GetTrainerScheduleAsync(Guid trainerId);
    Task<ResponseDto<PersonalScheduleDto>> GetGlobalScheduleAsync();
}
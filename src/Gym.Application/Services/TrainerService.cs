using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Trainers;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Gym.Application.Services;

public class TrainerService : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<TrainerDto>> GetByIdAsync(Guid id)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);

        if (trainer == null || trainer.IsDeleted)
        {
            return ResponseDto<TrainerDto>.FailureResult("Trainer not found");
        }

        var trainerDto = _mapper.Map<TrainerDto>(trainer);
        return ResponseDto<TrainerDto>.SuccessResult(trainerDto);
    }

    public async Task<ResponseDto<List<TrainerDto>>> GetAllAsync()
    {
        var trainers = await _unitOfWork.Trainers.GetAllAsync();
        var activeTrainers = trainers
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.FullName)
            .ToList();

        var trainerDtos = _mapper.Map<List<TrainerDto>>(activeTrainers);
        return ResponseDto<List<TrainerDto>>.SuccessResult(trainerDtos);
    }

    public async Task<ResponseDto<TrainerDto>> CreateAsync(CreateTrainerDto dto)
    {
        // Check duplicate email
        if (!string.IsNullOrEmpty(dto.Email))
        {
            var existingByEmail = await _unitOfWork.Trainers.FindAsync(t =>
                t.Email == dto.Email && !t.IsDeleted);

            if (existingByEmail.Any())
            {
                return ResponseDto<TrainerDto>.FailureResult("Email already exists");
            }
        }

        var trainer = _mapper.Map<Trainer>(dto);
        
        // Auto-generate TrainerCode if not provided
        if (string.IsNullOrEmpty(trainer.TrainerCode))
        {
            var count = await _unitOfWork.Trainers.GetQueryable().CountAsync();
            trainer.TrainerCode = $"PT{(count + 1):D3}"; // e.g., PT001, PT002
        }

        trainer.IsActive = true;
        trainer.HireDate = dto.HireDate ?? DateTime.Today;

        await _unitOfWork.Trainers.AddAsync(trainer);
        await _unitOfWork.SaveChangesAsync();

        var trainerDto = _mapper.Map<TrainerDto>(trainer);
        return ResponseDto<TrainerDto>.SuccessResult(trainerDto, "Trainer created successfully");
    }

    public async Task<ResponseDto<TrainerDto>> UpdateAsync(Guid id, UpdateTrainerDto dto)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);

        if (trainer == null || trainer.IsDeleted)
        {
            return ResponseDto<TrainerDto>.FailureResult("Trainer not found");
        }

        // Check duplicate email (exclude current trainer)
        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != trainer.Email)
        {
            var existingByEmail = await _unitOfWork.Trainers.FindAsync(t =>
                t.Email == dto.Email && !t.IsDeleted && t.Id != id);

            if (existingByEmail.Any())
            {
                return ResponseDto<TrainerDto>.FailureResult("Email already exists");
            }
        }

        _mapper.Map(dto, trainer);
        trainer.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Trainers.Update(trainer);
        await _unitOfWork.SaveChangesAsync();

        var trainerDto = _mapper.Map<TrainerDto>(trainer);
        return ResponseDto<TrainerDto>.SuccessResult(trainerDto, "Trainer updated successfully");
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);

        if (trainer == null || trainer.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Trainer not found");
        }

        // Check if trainer has active classes
        var classes = await _unitOfWork.Classes.FindAsync(c =>
            c.TrainerId == id && c.IsActive && !c.IsDeleted);

        if (classes.Any())
        {
            return ResponseDto<bool>.FailureResult("Cannot delete trainer with active classes. Please reassign or deactivate classes first.");
        }

        // Soft delete
        trainer.IsDeleted = true;
        trainer.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Trainers.Update(trainer);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Trainer deleted successfully");
    }

    public async Task<ResponseDto<List<TrainerDto>>> GetAvailableAsync()
    {
        var trainers = await _unitOfWork.Trainers.GetAvailableTrainersAsync();
        var trainerDtos = _mapper.Map<List<TrainerDto>>(trainers);
        return ResponseDto<List<TrainerDto>>.SuccessResult(trainerDtos);
    }

    public async Task<ResponseDto<List<TrainerAssignmentDto>>> GetAssignedMembersAsync(Guid trainerId)
    {
        var assignments = await _unitOfWork.TrainerMemberAssignments.GetQueryable()
            .Include(a => a.Member)
            .Include(a => a.Trainer)
            .Where(a => a.TrainerId == trainerId && !a.IsDeleted && a.IsActive)
            .ToListAsync();

        var dtos = _mapper.Map<List<TrainerAssignmentDto>>(assignments);
        return ResponseDto<List<TrainerAssignmentDto>>.SuccessResult(dtos);
    }

    public async Task<ResponseDto<TrainerAssignmentDto>> AssignMemberAsync(CreateTrainerAssignmentDto dto)
    {
        // Check if member exists
        var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
        if (member == null || member.IsDeleted)
            return ResponseDto<TrainerAssignmentDto>.FailureResult("Member not found");

        // Check if trainer exists
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId);
        if (trainer == null || trainer.IsDeleted)
            return ResponseDto<TrainerAssignmentDto>.FailureResult("Trainer not found");

        // Check if already assigned
        var existing = await _unitOfWork.TrainerMemberAssignments.FindAsync(a =>
            a.MemberId == dto.MemberId && a.TrainerId == dto.TrainerId && !a.IsDeleted && a.IsActive);

        if (existing.Any())
            return ResponseDto<TrainerAssignmentDto>.FailureResult("Member is already assigned to this trainer");

        var assignment = _mapper.Map<TrainerMemberAssignment>(dto);
        assignment.AssignedDate = DateTime.UtcNow;
        assignment.IsActive = true;

        await _unitOfWork.TrainerMemberAssignments.AddAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        // Load for mapping
        var result = await _unitOfWork.TrainerMemberAssignments.GetQueryable()
            .Include(a => a.Member)
            .Include(a => a.Trainer)
            .FirstOrDefaultAsync(a => a.Id == assignment.Id);

        return ResponseDto<TrainerAssignmentDto>.SuccessResult(_mapper.Map<TrainerAssignmentDto>(result), "Member assigned successfully");
    }

    public async Task<ResponseDto<bool>> RemoveAssignmentAsync(Guid assignmentId)
    {
        var assignment = await _unitOfWork.TrainerMemberAssignments.GetByIdAsync(assignmentId);
        if (assignment == null || assignment.IsDeleted)
            return ResponseDto<bool>.FailureResult("Assignment not found");

        assignment.IsDeleted = true;
        assignment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.TrainerMemberAssignments.Update(assignment);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Assignment removed successfully");
    }

    public async Task<ResponseDto<PersonalScheduleDto>> GetPersonalScheduleAsync(Guid userId, string? email = null, string? fullName = null, bool isAdmin = false)
    {
        var trainer = await _unitOfWork.Trainers.GetByUserIdAsync(userId);
        
        // --- FALLBACK LÀM MỀM LUỒNG DỮ LIỆU ---
        // 1. Thử tìm theo Email (Ưu tiên nhất)
        if (trainer == null && !string.IsNullOrEmpty(email))
        {
            var normalizedEmail = email.Trim().ToLower();
            var trainersByEmail = await _unitOfWork.Trainers.FindAsync(t => 
                t.Email != null && t.Email.Trim().ToLower() == normalizedEmail && !t.IsDeleted);
            
            trainer = trainersByEmail.FirstOrDefault();
        }

        // 2. Thử tìm theo Tên đầy đủ (Nếu email không khớp)
        if (trainer == null && !string.IsNullOrEmpty(fullName))
        {
            var normalizedName = fullName.Trim().ToLower();
            var trainersByName = await _unitOfWork.Trainers.FindAsync(t => 
                t.FullName != null && t.FullName.Trim().ToLower() == normalizedName && !t.IsDeleted);
            
            trainer = trainersByName.FirstOrDefault();
        }

        // Nếu tìm thấy qua bất kỳ fallback nào, thực hiện liên kết UserId cho các lần sau
        if (trainer != null && trainer.UserId == null)
        {
            trainer.UserId = userId;
            _unitOfWork.Trainers.Update(trainer);
            await _unitOfWork.SaveChangesAsync();
            
            // Nạp lại dữ liệu đầy đủ (Includes) sau khi link
            trainer = await _unitOfWork.Trainers.GetByUserIdAsync(userId);
        }

        // --- CHO PHÉP ADMIN TRUY CẬP VÀ XEM TOÀN BỘ DỮ LIỆU QUA HÀM NÀY NẾU CẦN ---
        if (trainer == null) {
            if (isAdmin) {
                return await GetGlobalScheduleAsync();
            }
            return ResponseDto<PersonalScheduleDto>.FailureResult("Tài khoản người dùng này chưa được liên kết với hồ sơ Huấn luyện viên.");
        }

        var dto = new PersonalScheduleDto
        {
            UserInfo = _mapper.Map<TrainerDto>(trainer),
            Classes = _mapper.Map<List<Gym.Application.DTOs.Classes.ClassDto>>(trainer.Classes.Where(c => !c.IsDeleted && c.IsActive).OrderBy(c => c.ScheduleDay).ThenBy(c => c.StartTime)),
            PersonalClients = _mapper.Map<List<TrainerAssignmentDto>>(trainer.TrainerMemberAssignments.Where(a => !a.IsDeleted && a.IsActive))
        };

        return ResponseDto<PersonalScheduleDto>.SuccessResult(dto);
    }

    public async Task<ResponseDto<PersonalScheduleDto>> GetTrainerScheduleAsync(Guid trainerId)
    {
        var trainer = await _unitOfWork.Trainers.GetQueryable()
            .Include(t => t.Classes)
            .Include(t => t.TrainerMemberAssignments)
                .ThenInclude(a => a.Member)
            .FirstOrDefaultAsync(t => t.Id == trainerId && !t.IsDeleted);

        if (trainer == null)
            return ResponseDto<PersonalScheduleDto>.FailureResult("Trainer not found.");

        var dto = new PersonalScheduleDto
        {
            UserInfo = _mapper.Map<TrainerDto>(trainer),
            Classes = _mapper.Map<List<Gym.Application.DTOs.Classes.ClassDto>>(trainer.Classes.Where(c => !c.IsDeleted && c.IsActive).OrderBy(c => c.ScheduleDay).ThenBy(c => c.StartTime)),
            PersonalClients = _mapper.Map<List<TrainerAssignmentDto>>(trainer.TrainerMemberAssignments.Where(a => !a.IsDeleted && a.IsActive))
        };

        return ResponseDto<PersonalScheduleDto>.SuccessResult(dto);
    }

    public async Task<ResponseDto<PersonalScheduleDto>> GetGlobalScheduleAsync()
    {
        try 
        {
            var classes = await _unitOfWork.Classes.GetQueryable()
                .Include(c => c.Trainer)
                .Where(c => !c.IsDeleted && c.IsActive)
                .OrderBy(c => c.ScheduleDay).ThenBy(c => c.StartTime)
                .ToListAsync();

            var assignments = await _unitOfWork.TrainerMemberAssignments.GetQueryable()
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Where(a => !a.IsDeleted && a.IsActive)
                .ToListAsync();

            var dto = new PersonalScheduleDto
            {
                UserInfo = new TrainerDto { FullName = "Tất cả Hệ thống" },
                Classes = _mapper.Map<List<Gym.Application.DTOs.Classes.ClassDto>>(classes),
                PersonalClients = _mapper.Map<List<TrainerAssignmentDto>>(assignments)
            };

            return ResponseDto<PersonalScheduleDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return ResponseDto<PersonalScheduleDto>.FailureResult($"Global schedule error: {ex.Message}");
        }
    }
}
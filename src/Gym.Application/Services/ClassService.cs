using AutoMapper;
using Gym.Application.DTOs.Classes;
using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Gym.Application.Services;

public class ClassService : IClassService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClassService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<ClassDto>> GetByIdAsync(Guid id)
    {
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(id);

        if (classEntity == null || classEntity.IsDeleted)
        {
            return ResponseDto<ClassDto>.FailureResult("Class not found");
        }

        var classDto = _mapper.Map<ClassDto>(classEntity);
        return ResponseDto<ClassDto>.SuccessResult(classDto);
    }

    public async Task<ResponseDto<List<ClassDto>>> GetAllAsync()
    {
        var classes = await _unitOfWork.Classes.GetAllAsync();
        var activeClasses = classes
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.ScheduleDay)
            .ThenBy(c => c.StartTime)
            .ToList();

        var classDtos = _mapper.Map<List<ClassDto>>(activeClasses);
        return ResponseDto<List<ClassDto>>.SuccessResult(classDtos);
    }

    public async Task<ResponseDto<List<ClassDto>>> GetActiveClassesAsync()
    {
        var classes = await _unitOfWork.Classes.GetAllAsync();
        var activeClasses = classes
            .Where(c => !c.IsDeleted && c.IsActive)
            .OrderBy(c => c.ScheduleDay)
            .ThenBy(c => c.StartTime)
            .ToList();

        var classDtos = _mapper.Map<List<ClassDto>>(activeClasses);
        return ResponseDto<List<ClassDto>>.SuccessResult(classDtos);
    }

    public async Task<ResponseDto<ClassDto>> CreateAsync(CreateClassDto dto)
    {
        // Validate trainer exists
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId);
        if (trainer == null || trainer.IsDeleted)
        {
            return ResponseDto<ClassDto>.FailureResult("Trainer not found");
        }

        if (!trainer.IsActive)
        {
            return ResponseDto<ClassDto>.FailureResult("Trainer is not active");
        }

        // Validate time
        if (dto.EndTime <= dto.StartTime)
        {
            return ResponseDto<ClassDto>.FailureResult("End time must be after start time");
        }

        var classEntity = _mapper.Map<Class>(dto);
        classEntity.IsActive = true;
        classEntity.CurrentEnrollment = 0;

        await _unitOfWork.Classes.AddAsync(classEntity);
        await _unitOfWork.SaveChangesAsync();

        var classDto = _mapper.Map<ClassDto>(classEntity);
        return ResponseDto<ClassDto>.SuccessResult(classDto, "Class created successfully");
    }

    public async Task<ResponseDto<ClassDto>> UpdateAsync(Guid id, UpdateClassDto dto)
    {
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(id);

        if (classEntity == null || classEntity.IsDeleted)
        {
            return ResponseDto<ClassDto>.FailureResult("Class not found");
        }

        // Validate trainer exists
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId);
        if (trainer == null || trainer.IsDeleted)
        {
            return ResponseDto<ClassDto>.FailureResult("Trainer not found");
        }

        // Validate time
        if (dto.EndTime <= dto.StartTime)
        {
            return ResponseDto<ClassDto>.FailureResult("End time must be after start time");
        }

        // Check if reducing capacity below current enrollment
        if (dto.MaxCapacity < classEntity.CurrentEnrollment)
        {
            return ResponseDto<ClassDto>.FailureResult($"Cannot reduce capacity below current enrollment ({classEntity.CurrentEnrollment})");
        }

        _mapper.Map(dto, classEntity);
        classEntity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Classes.Update(classEntity);
        await _unitOfWork.SaveChangesAsync();

        var classDto = _mapper.Map<ClassDto>(classEntity);
        return ResponseDto<ClassDto>.SuccessResult(classDto, "Class updated successfully");
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(id);

        if (classEntity == null || classEntity.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Class not found");
        }

        // Check if class has enrollments
        if (classEntity.CurrentEnrollment > 0)
        {
            return ResponseDto<bool>.FailureResult("Cannot delete class with active enrollments. Please unenroll all members first.");
        }

        // Soft delete
        classEntity.IsDeleted = true;
        classEntity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Classes.Update(classEntity);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Class deleted successfully");
    }

    public async Task<ResponseDto<List<ClassDto>>> GetUpcomingClassesAsync()
    {
        var classes = await _unitOfWork.Classes.GetUpcomingClassesAsync();
        var classDtos = _mapper.Map<List<ClassDto>>(classes);
        return ResponseDto<List<ClassDto>>.SuccessResult(classDtos);
    }

    public async Task<ResponseDto<bool>> EnrollMemberAsync(Guid classId, EnrollClassDto dto)
    {
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(classId);

        if (classEntity == null || classEntity.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Class not found");
        }

        if (!classEntity.IsActive)
        {
            return ResponseDto<bool>.FailureResult("Class is not active");
        }

        // Check capacity
        if (classEntity.CurrentEnrollment >= classEntity.MaxCapacity)
        {
            return ResponseDto<bool>.FailureResult("Class is full");
        }

        // Validate member exists
        var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
        if (member == null || member.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Member not found");
        }

        // Check if already enrolled
        var existingEnrollment = await _unitOfWork.ClassEnrollments.FindAsync(ce =>
            ce.ClassId == classId &&
            ce.MemberId == dto.MemberId &&
            !ce.IsDeleted);

        if (existingEnrollment.Any())
        {
            return ResponseDto<bool>.FailureResult("Member is already enrolled in this class");
        }

        // Create enrollment
        var enrollment = new ClassEnrollment
        {
            ClassId = classId,
            MemberId = dto.MemberId
        };

        await _unitOfWork.ClassEnrollments.AddAsync(enrollment);

        // Update class enrollment count
        classEntity.CurrentEnrollment++;
        _unitOfWork.Classes.Update(classEntity);

        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Member enrolled successfully");
    }

    public async Task<ResponseDto<bool>> UnenrollMemberAsync(Guid classId, Guid memberId)
    {
        var enrollment = await _unitOfWork.ClassEnrollments.FindAsync(ce =>
            ce.ClassId == classId &&
            ce.MemberId == memberId &&
            !ce.IsDeleted);

        var activeEnrollment = enrollment.FirstOrDefault();

        if (activeEnrollment == null)
        {
            return ResponseDto<bool>.FailureResult("Enrollment not found");
        }

        // Soft delete enrollment
        activeEnrollment.IsDeleted = true;
        activeEnrollment.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.ClassEnrollments.Update(activeEnrollment);

        // Update class enrollment count
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(classId);
        if (classEntity != null)
        {
            classEntity.CurrentEnrollment--;
            _unitOfWork.Classes.Update(classEntity);
        }

        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Member unenrolled successfully");
    }

    public async Task<ResponseDto<List<ClassAttendanceDto>>> GetEnrollmentsAsync(Guid classId)
    {
        var enrollments = await _unitOfWork.ClassEnrollments.GetQueryable()
            .Include(ce => ce.Member)
            .Where(ce => ce.ClassId == classId && !ce.IsDeleted)
            .OrderBy(ce => ce.Member!.FullName)
            .ToListAsync();

        var dtos = _mapper.Map<List<ClassAttendanceDto>>(enrollments);
        return ResponseDto<List<ClassAttendanceDto>>.SuccessResult(dtos);
    }

    public async Task<ResponseDto<bool>> MarkAttendanceAsync(MarkAttendanceDto dto)
    {
        var enrollment = await _unitOfWork.ClassEnrollments.GetByIdAsync(dto.EnrollmentId);
        if (enrollment == null || enrollment.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Enrollment not found");
        }

        enrollment.IsAttended = dto.IsPresent;
        enrollment.AttendanceDate = dto.IsPresent ? DateTime.UtcNow : null;
        enrollment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.ClassEnrollments.Update(enrollment);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, dto.IsPresent ? "Marked as present" : "Marked as absent");
    }
}
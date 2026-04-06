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
    private readonly IAuditLogService _auditLogService;
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

    public ClassService(IUnitOfWork unitOfWork, IMapper mapper, IAuditLogService auditLogService, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseDto<ClassDto>> GetByIdAsync(Guid id)
    {
        var classEntity = await _unitOfWork.Classes.GetQueryable()
            .Include(c => c.Trainer)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (classEntity == null)
        {
            return ResponseDto<ClassDto>.FailureResult("Không tìm thấy lớp học.");
        }

        var classDto = _mapper.Map<ClassDto>(classEntity);
        return ResponseDto<ClassDto>.SuccessResult(classDto);
    }

    public async Task<ResponseDto<List<ClassDto>>> GetAllAsync()
    {
        var classesQuery = _unitOfWork.Classes.GetQueryable()
            .Include(c => c.Trainer)
            .Where(c => !c.IsDeleted);

        // NGHIỆP VỤ (WF - 2.4): HLV chỉ thấy lớp của chính mình
        var user = _httpContextAccessor.HttpContext?.User;
        var role = user?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (role == "Trainer")
        {
            var userIdStr = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdStr, out var userId))
            {
                var trainers = await _unitOfWork.Trainers.FindAsync(t => t.UserId == userId);
                var trainer = trainers.FirstOrDefault();
                if (trainer != null)
                {
                    classesQuery = classesQuery.Where(c => c.TrainerId == trainer.Id);
                }
            }
        }

        var activeClassesFromDb = await classesQuery.ToListAsync();

        var activeClasses = activeClassesFromDb
            .OrderBy(c => c.ScheduleDay)
            .ThenBy(c => c.StartTime)
            .ToList();

        var classDtos = _mapper.Map<List<ClassDto>>(activeClasses);
        return ResponseDto<List<ClassDto>>.SuccessResult(classDtos);
    }

    public async Task<ResponseDto<List<ClassDto>>> GetActiveClassesAsync()
    {
        var activeClassesFromDb = await _unitOfWork.Classes.GetQueryable()
            .Include(c => c.Trainer)
            .Where(c => !c.IsDeleted && c.IsActive)
            .ToListAsync();

        var activeClasses = activeClassesFromDb
            .OrderBy(c => c.ScheduleDay)
            .ThenBy(c => c.StartTime)
            .ToList();

        var classDtos = _mapper.Map<List<ClassDto>>(activeClasses);
        return ResponseDto<List<ClassDto>>.SuccessResult(classDtos);
    }

    public async Task<ResponseDto<ClassDto>> CreateAsync(CreateClassDto dto)
    {
        // NGHIỆP VỤ (WF - 2.4): Chỉ Admin/Manager được tạo lớp
        var role = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (role != "Admin" && role != "Manager")
        {
            return ResponseDto<ClassDto>.FailureResult("Chỉ Quản lý mới có quyền tạo lịch lớp học.");
        }
        // Validate trainer exists
        if (dto.TrainerId.HasValue)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId.Value);
            if (trainer == null || trainer.IsDeleted)
            {
                return ResponseDto<ClassDto>.FailureResult($"LỖI: HLV (ID {dto.TrainerId}) không tồn tại. Vui lòng chọn lại HLV khác.");
            }

            if (!trainer.IsActive)
            {
                return ResponseDto<ClassDto>.FailureResult("Trainer is not active");
            }
            classEntity.TrainerId = trainer.Id;
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

        // Refetch with trainer name
        return await GetByIdAsync(classEntity.Id);
    }

    public async Task<ResponseDto<ClassDto>> UpdateAsync(Guid id, UpdateClassDto dto)
    {
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(id);

        if (classEntity == null || classEntity.IsDeleted)
        {
            return ResponseDto<ClassDto>.FailureResult("Class not found");
        }

        // Chụp lại trạng thái cũ để so sánh nhật ký
        var oldValues = new { 
            classEntity.ClassName, 
            classEntity.MaxCapacity, 
            classEntity.StartTime, 
            classEntity.EndTime, 
            classEntity.ScheduleDay, 
            classEntity.IsActive,
            classEntity.TrainerId
        };

        // Validate trainer exists
        if (dto.TrainerId.HasValue)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId.Value);
            if (trainer == null || trainer.IsDeleted)
            {
                return ResponseDto<ClassDto>.FailureResult($"LỖI: HLV (ID {dto.TrainerId}) không tồn tại. Vui lòng chọn lại HLV khác.");
            }
            classEntity.TrainerId = trainer.Id;
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
        var result = await _unitOfWork.SaveChangesAsync();

        if (result > 0)
        {
            // Ghi nhật ký chi tiết
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var newValues = new { 
                classEntity.ClassName, 
                classEntity.MaxCapacity, 
                classEntity.StartTime, 
                classEntity.EndTime, 
                classEntity.ScheduleDay, 
                classEntity.IsActive,
                classEntity.TrainerId
            };
            await _auditLogService.LogAsync(userIdStr ?? "System", "Modified", "Class", oldValues, newValues);
        }

        return await GetByIdAsync(classEntity.Id);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var classEntity = await _unitOfWork.Classes.GetByIdAsync(id);

        if (classEntity == null || classEntity.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Không tìm thấy lớp học.");
        }

        // --- TỰ ĐỘNG GỠ HỌC VIÊN (SOFT DELETE ENROLLMENTS) ---
        var enrollments = await _unitOfWork.ClassEnrollments.FindAsync(ce => ce.ClassId == id && !ce.IsDeleted);
        if (enrollments.Any())
        {
            foreach (var enrollment in enrollments)
            {
                enrollment.IsDeleted = true;
                enrollment.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.ClassEnrollments.Update(enrollment);
            }
        }

        // Soft delete class
        classEntity.IsDeleted = true;
        classEntity.CurrentEnrollment = 0; // Đưa về 0
        classEntity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Classes.Update(classEntity);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Đã xóa lớp học thành công (đã tự động gỡ các học viên cũ).");
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
        // NGHIỆP VỤ (WF - 2.4): Chỉ HLV của lớp hoặc Quản lý được điểm danh
        var user = _httpContextAccessor.HttpContext?.User;
        var role = user?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        
        var enrollment = await _unitOfWork.ClassEnrollments
            .GetQueryable()
            .Include(ce => ce.Class)
            .FirstOrDefaultAsync(ce => ce.Id == dto.EnrollmentId);

        if (enrollment == null || enrollment.IsDeleted)
        {
            return ResponseDto<bool>.FailureResult("Không tìm thấy thông tin đăng ký lớp.");
        }

        if (role == "Trainer")
        {
             var userIdStr = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
             if (Guid.TryParse(userIdStr, out var userId))
             {
                 var trainers = await _unitOfWork.Trainers.FindAsync(t => t.UserId == userId);
                 var trainer = trainers.FirstOrDefault();
                 if (trainer != null && enrollment.Class != null && enrollment.Class.TrainerId != trainer.Id)
                 {
                     return ResponseDto<bool>.FailureResult("Bạn chỉ có quyền điểm danh cho lớp học do bạn phụ trách.");
                 }
             }
        }

        enrollment.IsAttended = dto.IsPresent;
        enrollment.AttendanceDate = dto.IsPresent ? DateTime.UtcNow : null;
        enrollment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.ClassEnrollments.Update(enrollment);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, dto.IsPresent ? "Marked as present" : "Marked as absent");
    }
}

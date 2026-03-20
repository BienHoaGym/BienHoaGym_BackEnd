using AutoMapper;
using Gym.Application.Interfaces;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Equipment;
using Gym.Application.Interfaces.Repositories;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Services;

public class EquipmentCategoryService : IEquipmentCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EquipmentCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<EquipmentCategoryDto>>> GetAllAsync(string? searchTerm = null, string? group = null)
    {
        var query = _unitOfWork.EquipmentCategories.GetQueryable()
            .Include(c => c.Equipments)
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Code.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(group))
        {
            query = query.Where(c => c.Group == group);
        }

        var categories = await query.ToListAsync();

        var dtos = categories.Select(c => {
            var dto = _mapper.Map<EquipmentCategoryDto>(c);
            dto.EquipmentCount = c.Equipments.Count(e => !e.IsDeleted);
            return dto;
        }).ToList();

        return ResponseDto<List<EquipmentCategoryDto>>.SuccessResult(dtos);
    }

    public async Task<ResponseDto<EquipmentCategoryDto>> GetByIdAsync(Guid id)
    {
        var category = await _unitOfWork.EquipmentCategories.GetQueryable()
            .Include(c => c.Equipments)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (category == null) return ResponseDto<EquipmentCategoryDto>.FailureResult("Không tìm thấy loại thiết bị");

        var dto = _mapper.Map<EquipmentCategoryDto>(category);
        dto.EquipmentCount = category.Equipments.Count(e => !e.IsDeleted);
        return ResponseDto<EquipmentCategoryDto>.SuccessResult(dto);
    }

    public async Task<ResponseDto<EquipmentCategoryDto>> CreateAsync(CreateCategoryDto dto)
    {
        // Tự động sinh mã nếu để trống
        if (string.IsNullOrWhiteSpace(dto.Code))
        {
            var count = await _unitOfWork.EquipmentCategories.GetQueryable().CountAsync();
            dto.Code = $"EQCAT{(count + 1):D3}";
        }

        // Kiểm tra mã đã tồn tại chưa
        var exists = await _unitOfWork.EquipmentCategories.GetQueryable()
            .AnyAsync(c => c.Code == dto.Code && !c.IsDeleted);
        if (exists) return ResponseDto<EquipmentCategoryDto>.FailureResult("Mã loại thiết bị đã tồn tại");

        var category = _mapper.Map<EquipmentCategory>(dto);
        await _unitOfWork.EquipmentCategories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<EquipmentCategoryDto>.SuccessResult(_mapper.Map<EquipmentCategoryDto>(category));
    }

    public async Task<ResponseDto<EquipmentCategoryDto>> UpdateAsync(Guid id, CreateCategoryDto dto)
    {
        var category = await _unitOfWork.EquipmentCategories.GetByIdAsync(id);
        if (category == null || category.IsDeleted) return ResponseDto<EquipmentCategoryDto>.FailureResult("Không tìm thấy loại thiết bị");

        // Không cho phép sửa mã nếu đã có thiết bị liên kết
        if (category.Code != dto.Code)
        {
            var hasEquipment = await _unitOfWork.Equipments.GetQueryable()
                .AnyAsync(e => e.CategoryId == id && !e.IsDeleted);
            if (hasEquipment) return ResponseDto<EquipmentCategoryDto>.FailureResult("Không thể sửa mã của danh mục đang được sử dụng");
        }

        _mapper.Map(dto, category);
        _unitOfWork.EquipmentCategories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<EquipmentCategoryDto>.SuccessResult(_mapper.Map<EquipmentCategoryDto>(category));
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var hasEquipment = await _unitOfWork.Equipments.GetQueryable()
            .AnyAsync(e => e.CategoryId == id && !e.IsDeleted);
        if (hasEquipment) return ResponseDto<bool>.FailureResult("Không thể xóa loại thiết bị đang có máy liên kết");

        var category = await _unitOfWork.EquipmentCategories.GetByIdAsync(id);
        if (category == null) return ResponseDto<bool>.FailureResult("Không tìm thấy");

        category.IsDeleted = true;
        _unitOfWork.EquipmentCategories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Đã xóa danh mục");
    }
}

using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Equipment;

namespace Gym.Application.Interfaces.Services;

public interface IEquipmentCategoryService
{
    Task<ResponseDto<List<EquipmentCategoryDto>>> GetAllAsync(string? searchTerm = null, string? group = null);
    Task<ResponseDto<EquipmentCategoryDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<EquipmentCategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<ResponseDto<EquipmentCategoryDto>> UpdateAsync(Guid id, CreateCategoryDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
}

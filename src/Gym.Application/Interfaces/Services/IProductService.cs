using Gym.Application.DTOs.Billing;
using Gym.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces.Services;

public interface IProductService
{
    Task<ResponseDto<List<ProductDto>>> GetAllAsync(int? type = null, string? category = null);
    Task<ResponseDto<ProductDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<ResponseDto<ProductDto>> UpdateAsync(Guid id, CreateProductDto dto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
    Task<ResponseDto<bool>> ToggleStatusAsync(Guid id);
    
    Task<ResponseDto<List<string>>> GetCategoriesAsync();
}

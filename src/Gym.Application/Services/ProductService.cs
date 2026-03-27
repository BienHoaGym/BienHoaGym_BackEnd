using AutoMapper;
using Gym.Application.DTOs.Billing;
using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gym.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<ProductDto>>> GetAllAsync(int? type = null, string? category = null)
    {
        var query = _unitOfWork.Products.GetQueryable()
            .Where(p => !p.IsDeleted && p.Type != ProductType.Service)
            .Include(p => p.Provider)
            .AsNoTracking();

        if (type.HasValue)
            query = query.Where(p => (int)p.Type == type.Value);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        var products = await query.OrderBy(p => p.Name).ToListAsync();
        return ResponseDto<List<ProductDto>>.SuccessResult(_mapper.Map<List<ProductDto>>(products));
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetQueryable()
            .Include(p => p.Provider)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null) return ResponseDto<ProductDto>.FailureResult("Không tìm thấy sản phẩm");
        return ResponseDto<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<ResponseDto<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        product.CreatedAt = DateTime.UtcNow;

        // Tự động sinh SKU nếu để trống
        if (string.IsNullOrWhiteSpace(product.SKU))
        {
            var count = await _unitOfWork.Products.GetQueryable().CountAsync();
            product.SKU = $"PRD-{(count + 1):D4}";
        }
        
        // Logic cho dịch vụ: Không theo dõi tồn kho
        if (product.Type == ProductType.Service)
        {
            product.TrackInventory = false;
            product.StockQuantity = 0;
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        
        return ResponseDto<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(product), "Thêm sản phẩm thành công");
    }

    public async Task<ResponseDto<ProductDto>> UpdateAsync(Guid id, CreateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null || product.IsDeleted) return ResponseDto<ProductDto>.FailureResult("Không tìm thấy sản phẩm");

        _mapper.Map(dto, product);
        product.UpdatedAt = DateTime.UtcNow;

        if (product.Type == ProductType.Service)
        {
            product.TrackInventory = false;
        }

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(product), "Cập nhật sản phẩm thành công");
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

        // Soft delete
        product.IsDeleted = true;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, "Đã xóa sản phẩm");
    }

    public async Task<ResponseDto<bool>> ToggleStatusAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return ResponseDto<bool>.FailureResult("Không tìm thấy sản phẩm");

        product.IsActive = !product.IsActive;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResult(true, product.IsActive ? "Đã kích hoạt sản phẩm" : "Đã ngưng bán sản phẩm");
    }

    public async Task<ResponseDto<List<string>>> GetCategoriesAsync()
    {
        var categories = await _unitOfWork.Products.GetQueryable()
            .Where(p => !p.IsDeleted && !string.IsNullOrEmpty(p.Category))
            .Select(p => p.Category!)
            .Distinct()
            .ToListAsync();
        return ResponseDto<List<string>>.SuccessResult(categories);
    }
}

using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Providers;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Repositories;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Services;

public class ProviderService : IProviderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProviderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<ProviderSummaryDto>>> GetAllSummariesAsync(string? searchTerm = null, string? supplyType = null)
    {
        var query = _unitOfWork.Providers.GetQueryable()
            .Include(p => p.Equipments)
            .Include(p => p.Products)
            .Where(p => !p.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Code.Contains(searchTerm) || (p.Address != null && p.Address.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(supplyType))
        {
            query = query.Where(p => p.SupplyType == supplyType);
        }

        var providers = await query.ToListAsync();

        var summaries = providers.Select(p => new ProviderSummaryDto
        {
            Id = p.Id,
            Name = p.Name,
            Code = p.Code,
            PhoneNumber = p.PhoneNumber,
            Email = p.Email,
            Address = p.Address,
            SupplyType = p.SupplyType,
            IsActive = p.IsActive,
            EquipmentCount = p.Equipments.Count(e => !e.IsDeleted),
            ProductCount = p.Products.Count(pr => !pr.IsDeleted)
        }).ToList();

        return ResponseDto<List<ProviderSummaryDto>>.SuccessResult(summaries);
    }

    public async Task<ResponseDto<ProviderDto>> GetByIdAsync(Guid id)
    {
        var p = await _unitOfWork.Providers.GetByIdAsync(id);
        if (p == null || p.IsDeleted) return ResponseDto<ProviderDto>.FailureResult("Không tìm thấy");

        return ResponseDto<ProviderDto>.SuccessResult(_mapper.Map<ProviderDto>(p));
    }

    public async Task<ResponseDto<ProviderDto>> CreateAsync(CreateProviderDto dto)
    {
        // Tự động sinh mã nếu để trống
        if (string.IsNullOrWhiteSpace(dto.Code))
        {
            var count = await _unitOfWork.Providers.GetQueryable().CountAsync();
            dto.Code = $"NCC{(count + 1):D3}";
        }

        var provider = _mapper.Map<Provider>(dto);
        await _unitOfWork.Providers.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<ProviderDto>.SuccessResult(_mapper.Map<ProviderDto>(provider));
    }

    public async Task<ResponseDto<ProviderDto>> UpdateAsync(Guid id, CreateProviderDto dto)
    {
        var p = await _unitOfWork.Providers.GetByIdAsync(id);
        if (p == null || p.IsDeleted) return ResponseDto<ProviderDto>.FailureResult("Không tìm thấy");

        // Không cho phép sửa mã nếu đã có giao dịch
        if (p.Code != dto.Code)
        {
            var hasUsage = await _unitOfWork.Equipments.GetQueryable().AnyAsync(e => e.ProviderId == id && !e.IsDeleted) ||
                           await _unitOfWork.Products.GetQueryable().AnyAsync(pr => pr.ProviderId == id && !pr.IsDeleted);
            if (hasUsage) return ResponseDto<ProviderDto>.FailureResult("Không thể sửa mã của nhà cung cấp đã có giao dịch/thiết bị liên kết");
        }

        _mapper.Map(dto, p);
        _unitOfWork.Providers.Update(p);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<ProviderDto>.SuccessResult(_mapper.Map<ProviderDto>(p));
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        // Kiểm tra có dữ liệu liên quan không
        var hasUsage = await _unitOfWork.Equipments.GetQueryable().AnyAsync(e => e.ProviderId == id && !e.IsDeleted) ||
                       await _unitOfWork.Products.GetQueryable().AnyAsync(pr => pr.ProviderId == id && !pr.IsDeleted);

        if (hasUsage) return ResponseDto<bool>.FailureResult("Không thể xóa do đã có giao dịch/thiết bị liên kết. Chỉ có thể Ngừng hợp tác.");

        var p = await _unitOfWork.Providers.GetByIdAsync(id);
        if (p != null)
        {
            p.IsDeleted = true;
            _unitOfWork.Providers.Update(p);
            await _unitOfWork.SaveChangesAsync();
        }
        return ResponseDto<bool>.SuccessResult(true);
    }

    public async Task<ResponseDto<List<ProviderTransactionHistoryDto>>> GetTransactionHistoryAsync(Guid providerId)
    {
        var history = new List<ProviderTransactionHistoryDto>();

        // Lấy lịch sử bảo trì
        var maintenanceLogs = await _unitOfWork.MaintenanceLogs.GetQueryable()
            .Where(l => l.ProviderId == providerId)
            .Select(l => new ProviderTransactionHistoryDto {
                Id = l.Id,
                TransactionCode = $"MTN-{l.Id.ToString().Substring(0, 8)}",
                Type = "Bảo trì",
                Date = l.Date,
                TotalAmount = l.Cost,
                Status = l.Status.ToString()
            }).ToListAsync();
        
        history.AddRange(maintenanceLogs);

        // Lấy lịch sử nhập hàng (Giả định thông qua StockTransaction hoặc Products)
        var stockTransactions = await _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .Where(t => t.ProviderId == providerId)
            .ToListAsync();
        
        foreach(var st in stockTransactions) {
            history.Add(new ProviderTransactionHistoryDto {
                Id = st.Id,
                TransactionCode = $"STOCK-{st.Id.ToString().Substring(0, 8)}",
                Type = st.Type == Domain.Enums.StockTransactionType.Import ? "Nhập hàng" : "Xuất hàng",
                Date = st.Date,
                TotalAmount = (st.Product?.Price ?? 0) * st.Quantity,
                Status = "Hoàn tất"
            });
        }

        return ResponseDto<List<ProviderTransactionHistoryDto>>.SuccessResult(history.OrderByDescending(h => h.Date).ToList());
    }
}

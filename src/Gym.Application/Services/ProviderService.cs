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
            searchTerm = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || 
                                    (p.Code != null && p.Code.ToLower().Contains(searchTerm)) || 
                                    (p.Address != null && p.Address.ToLower().Contains(searchTerm)) ||
                                    (p.PhoneNumber != null && p.PhoneNumber.Contains(searchTerm)));
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
            Code = p.Code ?? "",
            PhoneNumber = p.PhoneNumber,
            Email = p.Email,
            Address = p.Address,
            ContactPerson = p.ContactPerson,
            TaxCode = p.TaxCode,
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
        if (p == null || p.IsDeleted) return ResponseDto<ProviderDto>.FailureResult("Không tìm thấy nhà cung cấp hoặc đã bị xóa");

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
        else
        {
            // Kiểm tra mã trùng
            var exists = await _unitOfWork.Providers.GetQueryable().AnyAsync(p => p.Code == dto.Code && !p.IsDeleted);
            if (exists) return ResponseDto<ProviderDto>.FailureResult($"Mã nhà cung cấp '{dto.Code}' đã tồn tại");
        }

        // Kiểm tra tên trùng (tùy chọn nhưng nên có)
        var nameExists = await _unitOfWork.Providers.GetQueryable().AnyAsync(p => p.Name == dto.Name && !p.IsDeleted);
        if (nameExists) return ResponseDto<ProviderDto>.FailureResult($"Tên nhà cung cấp '{dto.Name}' đã tồn tại trong hệ thống");

        var provider = _mapper.Map<Provider>(dto);
        await _unitOfWork.Providers.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync();
        
        return ResponseDto<ProviderDto>.SuccessResult(_mapper.Map<ProviderDto>(provider));
    }

    public async Task<ResponseDto<ProviderDto>> UpdateAsync(Guid id, CreateProviderDto dto)
    {
        var p = await _unitOfWork.Providers.GetByIdAsync(id);
        if (p == null || p.IsDeleted) return ResponseDto<ProviderDto>.FailureResult("Không tìm thấy nhà cung cấp");

        // Xử lý thay đổi mã
        string currentCode = (p.Code ?? "").Trim();
        string newCode = (dto.Code ?? "").Trim();
        
        if (!string.IsNullOrEmpty(newCode) && !string.Equals(currentCode, newCode, StringComparison.OrdinalIgnoreCase))
        {
            // Kiểm tra mã trùng với NCC khác
            var codeExists = await _unitOfWork.Providers.GetQueryable().AnyAsync(other => other.Code == newCode && other.Id != id && !other.IsDeleted);
            if (codeExists) return ResponseDto<ProviderDto>.FailureResult($"Mã '{newCode}' đã được sử dụng bởi đối tác khác");

            // Nếu đã có giao dịch thì không cho sửa mã để đảm bảo tính nhất quán (nếu mã được dùng làm ref)
            if (!string.IsNullOrEmpty(currentCode))
            {
                var hasUsage = await _unitOfWork.Equipments.GetQueryable().AnyAsync(e => e.ProviderId == id && !e.IsDeleted) ||
                               await _unitOfWork.Products.GetQueryable().AnyAsync(pr => pr.ProviderId == id && !pr.IsDeleted) ||
                               await _unitOfWork.StockTransactions.GetQueryable().AnyAsync(st => st.ProviderId == id && !st.IsDeleted) ||
                               await _unitOfWork.MaintenanceLogs.GetQueryable().AnyAsync(ml => ml.ProviderId == id && !ml.IsDeleted) ||
                               await _unitOfWork.EquipmentProviderHistories.GetQueryable().AnyAsync(h => (h.OldProviderId == id || h.NewProviderId == id) && !h.IsDeleted);
                               
                if (hasUsage) return ResponseDto<ProviderDto>.FailureResult("Không thể sửa mã của nhà cung cấp đã có giao dịch hoặc dữ liệu liên kết");
            }
        }

        // Cập nhật thông tin
        _mapper.Map(dto, p);
        _unitOfWork.Providers.Update(p);
        await _unitOfWork.SaveChangesAsync();
        
        return ResponseDto<ProviderDto>.SuccessResult(_mapper.Map<ProviderDto>(p));
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        // Kiểm tra có dữ liệu liên quan không
        var hasUsage = await _unitOfWork.Equipments.GetQueryable().AnyAsync(e => e.ProviderId == id && !e.IsDeleted) ||
                       await _unitOfWork.Products.GetQueryable().AnyAsync(pr => pr.ProviderId == id && !pr.IsDeleted) ||
                       await _unitOfWork.StockTransactions.GetQueryable().AnyAsync(st => st.ProviderId == id && !st.IsDeleted) ||
                       await _unitOfWork.MaintenanceLogs.GetQueryable().AnyAsync(ml => ml.ProviderId == id && !ml.IsDeleted) ||
                       await _unitOfWork.EquipmentProviderHistories.GetQueryable().AnyAsync(h => (h.OldProviderId == id || h.NewProviderId == id) && !h.IsDeleted);

        if (hasUsage) return ResponseDto<bool>.FailureResult("Không thể xóa hoàn toàn nhà cung cấp đã có lịch sử giao dịch hoặc thiết bị/sản phẩm liên kết. Vui lòng chuyển trạng thái sang 'Ngừng hợp tác'.");

        var p = await _unitOfWork.Providers.GetByIdAsync(id);
        if (p == null || p.IsDeleted) return ResponseDto<bool>.FailureResult("Không tìm thấy nhà cung cấp");

        p.IsDeleted = true;
        _unitOfWork.Providers.Update(p);
        await _unitOfWork.SaveChangesAsync();
        
        return ResponseDto<bool>.SuccessResult(true);
    }

    public async Task<ResponseDto<List<ProviderTransactionHistoryDto>>> GetTransactionHistoryAsync(Guid providerId)
    {
        var history = new List<ProviderTransactionHistoryDto>();

        // Lấy lịch sử bảo trì
        var maintenanceLogs = await _unitOfWork.MaintenanceLogs.GetQueryable()
            .Where(l => l.ProviderId == providerId && !l.IsDeleted)
            .OrderByDescending(l => l.Date)
            .Select(l => new ProviderTransactionHistoryDto {
                Id = l.Id,
                TransactionCode = $"MTN-{l.Id.ToString().Substring(0, 8).ToUpper()}",
                Type = "Bảo trì thiết bị",
                Date = l.Date,
                Note = l.Description,
                TotalAmount = l.Cost,
                PaidAmount = l.Cost, // Giả định bảo trì đã trả đủ
                Status = l.Status == Domain.Enums.MaintenanceStatus.Completed ? "Hoàn tất" : "Đang xử lý"
            }).ToListAsync();
        
        history.AddRange(maintenanceLogs);

        // Lấy lịch sử nhập hàng
        var stockTransactions = await _unitOfWork.StockTransactions.GetQueryable()
            .Include(t => t.Product)
            .Where(t => t.ProviderId == providerId && !t.IsDeleted)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
        
        foreach(var st in stockTransactions) {
            history.Add(new ProviderTransactionHistoryDto {
                Id = st.Id,
                TransactionCode = string.IsNullOrEmpty(st.ReferenceNumber) ? $"STK-{st.Id.ToString().Substring(0, 8).ToUpper()}" : st.ReferenceNumber,
                Type = st.Type == Domain.Enums.StockTransactionType.Import ? "Nhập hàng" : "Xuất hàng/Trả hàng",
                Date = st.Date,
                Note = st.Note ?? (st.Product != null ? $"Nhập {st.Quantity} {st.Product.Name}" : ""),
                TotalAmount = st.TotalAmount > 0 ? st.TotalAmount : (st.UnitPrice * st.Quantity),
                PaidAmount = st.PaidAmount > 0 ? st.PaidAmount : (st.TotalAmount > 0 ? 0 : (st.UnitPrice * st.Quantity)),
                Status = "Hoàn tất"
            });
        }

        // Lấy lịch sử mua thiết bị
        var equipmentTransactions = await _unitOfWork.EquipmentTransactions.GetQueryable()
            .Include(t => t.Equipment)
            .Where(t => t.Equipment.ProviderId == providerId && t.Type == Domain.Enums.EquipmentTransactionType.Purchase && !t.IsDeleted)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
        
        foreach (var et in equipmentTransactions) {
            history.Add(new ProviderTransactionHistoryDto {
                Id = et.Id,
                TransactionCode = $"EQP-{et.Id.ToString().Substring(0, 8).ToUpper()}",
                Type = "Mua thiết bị",
                Date = et.Date,
                Note = et.Note ?? $"Mua mới {et.Equipment?.Name}",
                TotalAmount = et.TotalAmount > 0 ? et.TotalAmount : (et.Quantity * (et.Equipment?.PurchasePrice ?? 0)),
                PaidAmount = et.PaidAmount > 0 ? et.PaidAmount : (et.TotalAmount > 0 ? 0 : (et.Quantity * (et.Equipment?.PurchasePrice ?? 0))),
                Status = "Hoàn tất"
            });
        }

        return ResponseDto<List<ProviderTransactionHistoryDto>>.SuccessResult(history.OrderByDescending(h => h.Date).ToList());
    }

    public async Task<ResponseDto<List<Gym.Application.DTOs.Billing.ProductDto>>> GetProductsAsync(Guid providerId)
    {
        var products = await _unitOfWork.Products.GetQueryable()
            .Where(p => p.ProviderId == providerId && !p.IsDeleted)
            .ToListAsync();
        
        return ResponseDto<List<Gym.Application.DTOs.Billing.ProductDto>>.SuccessResult(_mapper.Map<List<Gym.Application.DTOs.Billing.ProductDto>>(products));
    }

    public async Task<ResponseDto<List<Gym.Application.DTOs.Equipment.EquipmentDto>>> GetEquipmentsAsync(Guid providerId)
    {
        var equipments = await _unitOfWork.Equipments.GetQueryable()
            .Include(e => e.EquipmentCategory)
            .Where(e => e.ProviderId == providerId && !e.IsDeleted)
            .ToListAsync();
        
        return ResponseDto<List<Gym.Application.DTOs.Equipment.EquipmentDto>>.SuccessResult(_mapper.Map<List<Gym.Application.DTOs.Equipment.EquipmentDto>>(equipments));
    }

    public async Task<ResponseDto<bool>> PayDebtAsync(CreateProviderPaymentDto dto)
    {
        try
        {
            var provider = await _unitOfWork.Providers.GetByIdAsync(dto.ProviderId);
            if (provider == null) return ResponseDto<bool>.FailureResult("Không tìm thấy nhà cung cấp");

            // 1. Tạo bản ghi thanh toán (Phiếu chi)
            var payment = new ProviderPayment
            {
                ProviderId = dto.ProviderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Note = dto.Note,
                Date = DateTime.UtcNow,
                ReferenceNumber = dto.ReferenceNumber,
                StockTransactionId = dto.StockTransactionId,
                EquipmentTransactionId = dto.EquipmentTransactionId
            };

            await _unitOfWork.ProviderPayments.AddAsync(payment);

            // 2. Cập nhật số tiền đã thanh toán trong giao dịch gốc (nếu có)
            if (dto.StockTransactionId.HasValue)
            {
                var stockTx = await _unitOfWork.StockTransactions.GetByIdAsync(dto.StockTransactionId.Value);
                if (stockTx != null)
                {
                    stockTx.PaidAmount += dto.Amount;
                    _unitOfWork.StockTransactions.Update(stockTx);
                }
            }
            else if (dto.EquipmentTransactionId.HasValue)
            {
                var equipTx = await _unitOfWork.EquipmentTransactions.GetByIdAsync(dto.EquipmentTransactionId.Value);
                if (equipTx != null)
                {
                    equipTx.PaidAmount += dto.Amount;
                    _unitOfWork.EquipmentTransactions.Update(equipTx);
                }
            }

            // 3. Giảm tổng nợ của nhà cung cấp
            provider.TotalDebt -= dto.Amount;
            if (provider.TotalDebt < 0) provider.TotalDebt = 0;

            _unitOfWork.Providers.Update(provider);
            await _unitOfWork.SaveChangesAsync();

            return ResponseDto<bool>.SuccessResult(true, "Thanh toán nợ thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<bool>.FailureResult($"Lỗi khi thanh toán: {ex.Message}");
        }
    }
}

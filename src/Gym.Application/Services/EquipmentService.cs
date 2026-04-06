using AutoMapper;
using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Equipment;
using Gym.Application.DTOs.Inventory;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IInventoryService _inventoryService;

    public EquipmentService(IUnitOfWork unitOfWork, IMapper mapper, IInventoryService inventoryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _inventoryService = inventoryService;
    }

    public async Task<ResponseDto<List<EquipmentDto>>> GetEquipmentsAsync(Guid? categoryId = null, Guid? providerId = null, EquipmentStatus? status = null, string? location = null, string? searchTerm = null)
    {
        var query = _unitOfWork.Equipments.GetQueryable()
            .Include(e => e.EquipmentCategory)
            .Include(e => e.Provider)
            .Where(e => !e.IsDeleted)
            .AsQueryable();

        if (categoryId.HasValue) query = query.Where(e => e.CategoryId == categoryId.Value);
        if (providerId.HasValue) query = query.Where(e => e.ProviderId == providerId.Value);
        if (status.HasValue) query = query.Where(e => e.Status == status.Value);
        if (!string.IsNullOrWhiteSpace(location)) query = query.Where(e => e.Location != null && e.Location.Contains(location));
        if (!string.IsNullOrWhiteSpace(searchTerm)) query = query.Where(e => e.Name.Contains(searchTerm) || e.EquipmentCode.Contains(searchTerm) || (e.SerialNumber != null && e.SerialNumber.Contains(searchTerm)));

        var equipments = await query.ToListAsync();
        var dtos = _mapper.Map<List<EquipmentDto>>(equipments);
        
        return ResponseDto<List<EquipmentDto>>.SuccessResult(dtos);
    }

    public async Task<ResponseDto<EquipmentDto>> GetByIdAsync(Guid id)
    {
        var equipment = await _unitOfWork.Equipments.GetQueryable()
            .Include(e => e.EquipmentCategory)
            .Include(e => e.Provider)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        if (equipment == null) return ResponseDto<EquipmentDto>.FailureResult("Thiết bị không tồn tại");
        
        var dto = _mapper.Map<EquipmentDto>(equipment);
        
        return ResponseDto<EquipmentDto>.SuccessResult(dto);
    }

    public async Task<ResponseDto<EquipmentDto>> CreateEquipmentAsync(CreateEquipmentDto dto)
    {
        // Tự động sinh mã nếu để trống
        if (string.IsNullOrWhiteSpace(dto.EquipmentCode))
        {
            var count = await _unitOfWork.Equipments.GetQueryable().CountAsync();
            dto.EquipmentCode = $"EQ{(count + 1):D4}";
        }

        var equipment = _mapper.Map<Equipment>(dto);
        
        // Initialize depreciation & maintenance state
        RecalculateDepreciationState(equipment);
        UpdateMaintenanceSchedule(equipment);

        await _unitOfWork.Equipments.AddAsync(equipment);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<EquipmentDto>.SuccessResult(_mapper.Map<EquipmentDto>(equipment));
    }

    private void RecalculateDepreciationState(Equipment equipment)
    {
        if (equipment.UsefulLifeMonths > 0)
        {
            equipment.MonthlyDepreciationAmount = Math.Max(0, (equipment.PurchasePrice - equipment.SalvageValue) / equipment.UsefulLifeMonths);
        }
        else
        {
            equipment.MonthlyDepreciationAmount = 0;
        }

        // Snapshot is better updated when a Depreciation record is actually saved
        // But we initialize it here
        equipment.AccumulatedDepreciation = equipment.Depreciations?.Sum(d => d.Amount) ?? 0;
        equipment.RemainingValue = equipment.PurchasePrice - equipment.AccumulatedDepreciation;
        equipment.IsFullyDepreciated = equipment.RemainingValue <= equipment.SalvageValue && equipment.AccumulatedDepreciation > 0;
    }

    private void UpdateMaintenanceSchedule(Equipment equipment)
    {
        if (equipment.MaintenanceIntervalDays > 0)
        {
            // Nếu chưa bao giờ bảo trì, lấy ngày mua làm mốc. Nếu đã bảo trì, lấy ngày cuối làm mốc.
            var baseDate = equipment.LastMaintenanceDate ?? equipment.PurchaseDate;
            
            // Chỉ cập nhật nếu ngày kế tiếp đang trống hoặc mốc thời gian cũ hơn ngày dự kiến mới
            var expectedNextDate = baseDate.AddDays(equipment.MaintenanceIntervalDays);
            
            // Nếu ngày kế tiếp hiện tại đang trống HOẶC người dùng vừa đổi chu kỳ 
            // (Chúng ta ưu tiên cập nhật để khớp với chu kỳ mới)
            equipment.NextMaintenanceDate = expectedNextDate;
        }
    }

    public async Task<ResponseDto<EquipmentDto>> UpdateEquipmentAsync(Guid id, CreateEquipmentDto dto)
    {
        var equipment = await _unitOfWork.Equipments.GetQueryable()
            .Include(e => e.MaintenanceLogs)
            .Include(e => e.IncidentLogs)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (equipment == null) return ResponseDto<EquipmentDto>.FailureResult("Thiết bị không tồn tại");

        // Kiểm tra chặn sửa mã nếu đã có lịch sử (cho phép gán lần đầu nếu đang trống)
        string currentCode = (equipment.EquipmentCode ?? "").Trim();
        string newCode = (dto.EquipmentCode ?? "").Trim();
        
        if (!string.IsNullOrEmpty(currentCode) && !string.Equals(currentCode, newCode, StringComparison.OrdinalIgnoreCase))
        {
            var hasHistory = equipment.MaintenanceLogs.Count > 0 || equipment.IncidentLogs.Count > 0;
            if (hasHistory) return ResponseDto<EquipmentDto>.FailureResult("Không thể sửa mã thiết bị đã có lịch sử bảo trì hoặc sự cố");
        }

        // Lưu lịch sử nhà cung cấp nếu thay đổi
        if (equipment.ProviderId != dto.ProviderId)
        {
            var history = new EquipmentProviderHistory
            {
                EquipmentId = id,
                OldProviderId = equipment.ProviderId,
                NewProviderId = dto.ProviderId,
                ChangeDate = DateTime.UtcNow,
                Reason = "Cập nhật thông tin thiết bị"
            };
            await _unitOfWork.EquipmentProviderHistories.AddAsync(history);
        }

        // Lưu lịch sử điều chuyển nếu vị trí thay đổi
        if (equipment.Location != dto.Location)
        {
            var trans = new EquipmentTransaction
            {
                EquipmentId = id,
                Type = EquipmentTransactionType.Transfer,
                Quantity = equipment.Quantity,
                Date = DateTime.UtcNow,
                FromLocation = equipment.Location,
                ToLocation = dto.Location,
                Note = "Tự động ghi nhận khi cập nhật vị trí"
            };
            await _unitOfWork.EquipmentTransactions.AddAsync(trans);
        }

        _mapper.Map(dto, equipment);
        
        // Re-calculate schedules if relevant fields changed
        UpdateMaintenanceSchedule(equipment);
        RecalculateDepreciationState(equipment);

        _unitOfWork.Equipments.Update(equipment);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<EquipmentDto>.SuccessResult(_mapper.Map<EquipmentDto>(equipment));
    }

    public async Task<ResponseDto<bool>> DeleteEquipmentAsync(Guid id)
    {
        var equipment = await _unitOfWork.Equipments.GetQueryable()
            .Include(e => e.MaintenanceLogs)
            .Include(e => e.IncidentLogs)
            .Include(e => e.Transactions)
            .Include(e => e.Depreciations)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (equipment == null) return ResponseDto<bool>.FailureResult("Thiết bị không tồn tại");

        // Kiểm tra tất cả các loại lịch sử
        bool hasHistory = equipment.MaintenanceLogs.Any() || 
                         equipment.IncidentLogs.Any() || 
                         equipment.Transactions.Any() || 
                         equipment.Depreciations.Any();

        // Chỉ cho phép xóa thực sự (soft delete) nếu:
        // 1. Không có lịch sử gì
        // 2. HOẶC đã được Thanh lý (xác nhận kết thúc vòng đời)
        if (hasHistory && equipment.Status != EquipmentStatus.Liquidated)
        {
            return ResponseDto<bool>.FailureResult("Thiết bị này đã có dữ liệu lịch sử. Để đảm bảo tính toàn vẹn của báo cáo, bạn không thể xóa vĩnh viễn. Vui lòng sử dụng chức năng 'Thanh lý' để dừng theo dõi thiết bị này.");
        }

        equipment.IsDeleted = true;
        _unitOfWork.Equipments.Update(equipment);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Đã xóa thiết bị thành công");
    }

    public async Task<ResponseDto<bool>> LiquidateEquipmentAsync(Guid id)
    {
        var equipment = await _unitOfWork.Equipments.GetByIdAsync(id);
        if (equipment == null) return ResponseDto<bool>.FailureResult("Không tìm thấy");

        equipment.Status = EquipmentStatus.Liquidated;
        
        // Ghi nhận giao dịch thanh lý
        var trans = new EquipmentTransaction
        {
            EquipmentId = id,
            Type = EquipmentTransactionType.Liquidation,
            Quantity = equipment.Quantity,
            Date = DateTime.UtcNow,
            Note = "Ghi nhận thanh lý thiết bị"
        };
        await _unitOfWork.EquipmentTransactions.AddAsync(trans);

        _unitOfWork.Equipments.Update(equipment);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Đã thanh lý thiết bị");
    }

    public async Task<ResponseDto<List<EquipmentTransactionDto>>> GetTransactionsAsync(Guid? equipmentId = null)
    {
        var query = _unitOfWork.EquipmentTransactions.GetQueryable()
            .Include(t => t.Equipment)
            .AsQueryable();
        
        if (equipmentId.HasValue) query = query.Where(t => t.EquipmentId == equipmentId.Value);
        
        var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();
        return ResponseDto<List<EquipmentTransactionDto>>.SuccessResult(_mapper.Map<List<EquipmentTransactionDto>>(transactions));
    }

    public async Task<ResponseDto<bool>> RecordTransactionAsync(CreateEquipmentTransactionDto dto)
    {
        var equipment = await _unitOfWork.Equipments.GetByIdAsync(dto.EquipmentId);
        if (equipment == null) return ResponseDto<bool>.FailureResult("Không tìm thấy thiết bị");
        
        // Cập nhật số lượng dựa trên loại giao dịch
        if (dto.Type == EquipmentTransactionType.Purchase)
        {
            equipment.Quantity += dto.Quantity;
        }
        else if (dto.Type == EquipmentTransactionType.Liquidation)
        {
            equipment.Quantity = Math.Max(0, equipment.Quantity - dto.Quantity);
            if (equipment.Quantity == 0) equipment.Status = EquipmentStatus.Liquidated;
        }

        var transaction = _mapper.Map<EquipmentTransaction>(dto);
        await _unitOfWork.EquipmentTransactions.AddAsync(transaction);
        
        _unitOfWork.Equipments.Update(equipment);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Ghi nhận giao dịch thành công");
    }

    public async Task<ResponseDto<List<EquipmentProviderHistoryDto>>> GetProviderHistoryAsync(Guid equipmentId)
    {
        var history = await _unitOfWork.EquipmentProviderHistories.GetQueryable()
            .Include(h => h.OldProvider)
            .Include(h => h.NewProvider)
            .Where(h => h.EquipmentId == equipmentId)
            .OrderByDescending(h => h.ChangeDate)
            .ToListAsync();
        
        return ResponseDto<List<EquipmentProviderHistoryDto>>.SuccessResult(_mapper.Map<List<EquipmentProviderHistoryDto>>(history));
    }

    public async Task<ResponseDto<List<MaintenanceLogDto>>> GetMaintenanceLogsAsync(Guid? equipmentId = null)
    {
        var query = _unitOfWork.MaintenanceLogs.GetQueryable()
            .Include(l => l.Equipment)
            .AsQueryable();
        
        if (equipmentId.HasValue) query = query.Where(l => l.EquipmentId == equipmentId.Value);
        
        var logs = await query.OrderByDescending(l => l.Date).ToListAsync();
        return ResponseDto<List<MaintenanceLogDto>>.SuccessResult(_mapper.Map<List<MaintenanceLogDto>>(logs));
    }

    public async Task<ResponseDto<MaintenanceLogDto>> LogMaintenanceAsync(CreateMaintenanceLogDto dto)
    {
        var equipment = await _unitOfWork.Equipments.GetByIdAsync(dto.EquipmentId);
        if (equipment == null) return ResponseDto<MaintenanceLogDto>.FailureResult("Equipment not found");
        
        var log = _mapper.Map<MaintenanceLog>(dto);
        await _unitOfWork.MaintenanceLogs.AddAsync(log);

        // LUỒNG NGHIỆP VỤ LIÊN KẾT: Xuất vật tư từ Kho để bảo trì thiết bị
        if (dto.UsedMaterials != null && dto.UsedMaterials.Any())
        {
            foreach (var mat in dto.UsedMaterials)
            {
                var exportResult = await _inventoryService.InternalUseStockAsync(new CreateStockTransactionDto
                {
                    ProductId = mat.ProductId,
                    FromWarehouseId = mat.WarehouseId,
                    Quantity = mat.Quantity,
                    Note = $"Xuất vật tư bảo trì cho thiết bị: {equipment.Name} (Mã: {equipment.EquipmentCode})"
                });

                if (!exportResult.Success)
                {
                    // Có thể quyết định rollback hoặc báo lỗi phần này.
                    // Ở đây ta ghi chú vào Log nếu thất bại (ví dụ: hết hàng trong kho)
                    log.Description = (log.Description ?? "") + $" [CẢNH BÁO: Lỗi xuất vật tư ID {mat.ProductId}: {exportResult.Message}]";
                }
            }
        }

        // Update equipment maintenance dates and status if completed
        if (dto.Status == MaintenanceStatus.Completed)
        {
            // Safety check for existing items with 0 interval
            int interval = equipment.MaintenanceIntervalDays > 0 ? equipment.MaintenanceIntervalDays : 90;
            
            equipment.LastMaintenanceDate = dto.Date;
            equipment.NextMaintenanceDate = dto.Date.AddDays(interval);
            
            // Nếu thiết bị đang hỏng hoặc đang bảo trì, đưa về trạng thái hoạt động
            if (equipment.Status == EquipmentStatus.Broken || equipment.Status == EquipmentStatus.Maintenance)
            {
                equipment.Status = EquipmentStatus.Active;
            }
            
            _unitOfWork.Equipments.Update(equipment);
        }

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<MaintenanceLogDto>.SuccessResult(_mapper.Map<MaintenanceLogDto>(log), "Maintenance log created");
    }

    public async Task<ResponseDto<List<IncidentLogDto>>> GetIncidentLogsAsync(Guid? equipmentId = null)
    {
        var query = _unitOfWork.IncidentLogs.GetQueryable()
            .Include(l => l.Equipment)
            .AsQueryable();

        if (equipmentId.HasValue) query = query.Where(l => l.EquipmentId == equipmentId.Value);

        var logs = await query.OrderByDescending(l => l.Date).ToListAsync();
        return ResponseDto<List<IncidentLogDto>>.SuccessResult(_mapper.Map<List<IncidentLogDto>>(logs));
    }

    public async Task<ResponseDto<IncidentLogDto>> LogIncidentAsync(CreateIncidentLogDto dto)
    {
        var equipment = await _unitOfWork.Equipments.GetByIdAsync(dto.EquipmentId);
        if (equipment == null) return ResponseDto<IncidentLogDto>.FailureResult("Equipment not found");

        var log = _mapper.Map<IncidentLog>(dto);
        await _unitOfWork.IncidentLogs.AddAsync(log);
        
        // Optional: Update status to Broken
        if (dto.ResolutionStatus == "Open")
        {
            equipment.Status = EquipmentStatus.Broken;
            _unitOfWork.Equipments.Update(equipment);
        }

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<IncidentLogDto>.SuccessResult(_mapper.Map<IncidentLogDto>(log));
    }

    public async Task<ResponseDto<List<DepreciationDto>>> GetDepreciationsAsync(Guid? equipmentId = null)
    {
        var query = _unitOfWork.Depreciations.GetQueryable()
            .Include(d => d.Equipment)
            .AsQueryable();
        
        if (equipmentId.HasValue) query = query.Where(d => d.EquipmentId == equipmentId.Value);
        
        var result = await query.OrderByDescending(d => d.Date).ToListAsync();
        return ResponseDto<List<DepreciationDto>>.SuccessResult(_mapper.Map<List<DepreciationDto>>(result));
    }

    public async Task<ResponseDto<List<EquipmentDto>>> GetMaintenancePlanAsync()
    {
        // Get equipments that need maintenance in the next 7 days or are overdue
        var threshold = DateTime.UtcNow.AddDays(7);
        var equipments = await _unitOfWork.Equipments.FindAsync(e => 
            e.NextMaintenanceDate <= threshold || e.Status == EquipmentStatus.Broken);
            
        return ResponseDto<List<EquipmentDto>>.SuccessResult(_mapper.Map<List<EquipmentDto>>(equipments));
    }

    public async Task<ResponseDto<bool>> RecordDepreciationAsync(Guid equipmentId, int month, int year, string? note = null)
    {
        var equipment = await _unitOfWork.Equipments.GetByIdAsync(equipmentId);
        if (equipment == null) return ResponseDto<bool>.FailureResult("Không tìm thấy thiết bị");

        // Check if already recorded for this period
        var exists = await _unitOfWork.Depreciations.GetQueryable()
            .AnyAsync(d => d.EquipmentId == equipmentId && d.PeriodMonth == month && d.PeriodYear == year);
        if (exists) return ResponseDto<bool>.FailureResult($"Đã ghi nhận khấu hao cho tháng {month}/{year}");

        if (equipment.UsefulLifeMonths <= 0) return ResponseDto<bool>.FailureResult("Thời gian khấu hao chưa được cấu hình");

        // Calculate monthly amount: (OriginalPrice - SalvageValue) / UsefulLife
        decimal monthlyAmount = Math.Max(0, (equipment.PurchasePrice - equipment.SalvageValue) / equipment.UsefulLifeMonths);
        
        // Total already depreciated
        var totalDepreciated = await _unitOfWork.Depreciations.GetQueryable()
            .Where(d => d.EquipmentId == equipmentId)
            .Select(d => (double)d.Amount)
            .SumAsync();

        if ((decimal)totalDepreciated >= (equipment.PurchasePrice - equipment.SalvageValue))
            return ResponseDto<bool>.FailureResult("Thiết bị đã khấu hao hết");

        var dep = new Depreciation
        {
            EquipmentId = equipmentId,
            Amount = monthlyAmount,
            PeriodMonth = month,
            PeriodYear = year,
            RemainingValue = equipment.PurchasePrice - (decimal)totalDepreciated - monthlyAmount,
            Note = note ?? $"Khấu hao định kỳ tháng {month}/{year}",
            Date = DateTime.UtcNow
        };

        await _unitOfWork.Depreciations.AddAsync(dep);
        
        // Cập nhật Snapshot trên Equipment để xem báo cáo nhanh
        RecalculateDepreciationState(equipment);
        _unitOfWork.Equipments.Update(equipment);

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<bool>.SuccessResult(true, "Ghi nhận khấu hao thành công");
    }

    public async Task<ResponseDto<int>> BulkRecordDepreciationAsync(int month, int year)
    {
        var activeEquipments = await _unitOfWork.Equipments.GetQueryable()
            .Where(e => !e.IsDeleted && e.Status != EquipmentStatus.Liquidated)
            .ToListAsync();

        int count = 0;
        foreach (var eq in activeEquipments)
        {
            var res = await RecordDepreciationAsync(eq.Id, month, year);
            if (res.Success) count++;
        }

        return ResponseDto<int>.SuccessResult(count, $"Đã ghi nhận khấu hao cho {count}/{activeEquipments.Count} thiết bị.");
    }
}

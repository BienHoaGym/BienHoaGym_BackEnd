using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Equipment;
using Gym.Domain.Enums;

namespace Gym.Application.Interfaces.Services;

public interface IEquipmentService
{
    // Equipment Assets
    Task<ResponseDto<List<EquipmentDto>>> GetEquipmentsAsync(Guid? categoryId = null, Guid? providerId = null, EquipmentStatus? status = null, string? location = null, string? searchTerm = null);
    Task<ResponseDto<EquipmentDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<EquipmentDto>> CreateEquipmentAsync(CreateEquipmentDto dto);
    Task<ResponseDto<EquipmentDto>> UpdateEquipmentAsync(Guid id, CreateEquipmentDto dto);
    Task<ResponseDto<bool>> DeleteEquipmentAsync(Guid id);
    Task<ResponseDto<bool>> LiquidateEquipmentAsync(Guid id);

    
    // Transactions & History
    Task<ResponseDto<List<EquipmentTransactionDto>>> GetTransactionsAsync(Guid? equipmentId = null);
    Task<ResponseDto<bool>> RecordTransactionAsync(CreateEquipmentTransactionDto dto);
    Task<ResponseDto<List<EquipmentProviderHistoryDto>>> GetProviderHistoryAsync(Guid equipmentId);
    
    // Maintenance & Incidents
    Task<ResponseDto<List<MaintenanceLogDto>>> GetMaintenanceLogsAsync(Guid? equipmentId = null);
    Task<ResponseDto<MaintenanceLogDto>> LogMaintenanceAsync(CreateMaintenanceLogDto dto);
    Task<ResponseDto<List<IncidentLogDto>>> GetIncidentLogsAsync(Guid? equipmentId = null);
    Task<ResponseDto<IncidentLogDto>> LogIncidentAsync(CreateIncidentLogDto dto);
    
    // Depreciation
    Task<ResponseDto<List<DepreciationDto>>> GetDepreciationsAsync(Guid? equipmentId = null);
    Task<ResponseDto<int>> BulkRecordDepreciationAsync(int month, int year);
    Task<ResponseDto<bool>> RecordDepreciationAsync(Guid equipmentId, int month, int year, string? note = null);
    Task<ResponseDto<List<EquipmentDto>>> GetMaintenancePlanAsync();
}

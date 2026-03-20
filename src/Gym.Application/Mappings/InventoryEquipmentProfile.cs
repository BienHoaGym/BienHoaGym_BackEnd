using AutoMapper;
using Gym.Application.DTOs.Inventory;
using Gym.Application.DTOs.Equipment;
using Gym.Application.DTOs.Providers;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class InventoryEquipmentProfile : Profile
{
    public InventoryEquipmentProfile()
    {
        // Inventory
        CreateMap<Inventory, InventoryDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : ""));
        
        CreateMap<StockTransaction, StockTransactionDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : ""))
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""));
        CreateMap<CreateStockTransactionDto, StockTransaction>();

        // Equipment
        CreateMap<Equipment, EquipmentDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.EquipmentCategory != null ? s.EquipmentCategory.Name : ""))
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""));
        CreateMap<CreateEquipmentDto, Equipment>();

        CreateMap<EquipmentCategory, EquipmentCategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, EquipmentCategory>();
        
        CreateMap<Provider, ProviderDto>().ReverseMap();
        CreateMap<CreateProviderDto, Provider>();
        CreateMap<Provider, ProviderSummaryDto>();
        
        CreateMap<EquipmentTransaction, EquipmentTransactionDto>()
            .ForMember(d => d.EquipmentName, o => o.MapFrom(s => s.Equipment != null ? s.Equipment.Name : ""));
        CreateMap<CreateEquipmentTransactionDto, EquipmentTransaction>();
        
        CreateMap<MaintenanceLog, MaintenanceLogDto>()
            .ForMember(d => d.EquipmentName, o => o.MapFrom(s => s.Equipment != null ? s.Equipment.Name : ""))
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""));
        CreateMap<CreateMaintenanceLogDto, MaintenanceLog>();
        
        CreateMap<Depreciation, DepreciationDto>()
            .ForMember(d => d.EquipmentName, o => o.MapFrom(s => s.Equipment != null ? s.Equipment.Name : ""));

        CreateMap<IncidentLog, IncidentLogDto>()
            .ForMember(d => d.EquipmentName, o => o.MapFrom(s => s.Equipment != null ? s.Equipment.Name : ""));
        CreateMap<CreateIncidentLogDto, IncidentLog>();

        CreateMap<EquipmentProviderHistory, EquipmentProviderHistoryDto>()
            .ForMember(d => d.OldProviderName, o => o.MapFrom(s => s.OldProvider != null ? s.OldProvider.Name : ""))
            .ForMember(d => d.NewProviderName, o => o.MapFrom(s => s.NewProvider != null ? s.NewProvider.Name : ""));

        // Orders
        CreateMap<CreateOrderDto, Order>()
            .ForMember(d => d.OrderDetails, o => o.MapFrom(s => s.Details));
        CreateMap<CreateOrderDetailDto, OrderDetail>();
    }
}

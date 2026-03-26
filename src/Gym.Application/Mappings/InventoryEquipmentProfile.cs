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
        // Warehouse
        CreateMap<Warehouse, WarehouseDto>().ReverseMap();
        CreateMap<CreateWarehouseDto, Warehouse>();

        // Inventory
        CreateMap<Inventory, InventoryDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : ""))
            .ForMember(d => d.ProductSKU, o => o.MapFrom(s => s.Product != null ? s.Product.SKU : ""))
            .ForMember(d => d.MinStockThreshold, o => o.MapFrom(s => s.Product != null ? s.Product.MinStockThreshold : 0))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Product != null ? s.Product.Price : 0))
            .ForMember(d => d.CostPrice, o => o.MapFrom(s => s.Product != null ? s.Product.CostPrice : 0))
            .ForMember(d => d.WarehouseName, o => o.MapFrom(s => s.Warehouse != null ? s.Warehouse.Name : ""));
        
        CreateMap<StockTransaction, StockTransactionDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : ""))
            .ForMember(d => d.FromWarehouseName, o => o.MapFrom(s => s.FromWarehouse != null ? s.FromWarehouse.Name : ""))
            .ForMember(d => d.ToWarehouseName, o => o.MapFrom(s => s.ToWarehouse != null ? s.ToWarehouse.Name : ""))
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""))
            .ForMember(d => d.PerformedBy, o => o.MapFrom(s => s.PerformedBy))
            .ForMember(d => d.BeforeQuantity, o => o.MapFrom(s => s.BeforeQuantity))
            .ForMember(d => d.AfterQuantity, o => o.MapFrom(s => s.AfterQuantity));
        CreateMap<CreateStockTransactionDto, StockTransaction>();

        // Equipment
        CreateMap<Equipment, EquipmentDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.EquipmentCategory != null ? s.EquipmentCategory.Name : ""))
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""))
            .ForMember(d => d.CurrentBookValue, o => o.MapFrom(s => s.RemainingValue));
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
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""))
            .ForMember(d => d.Materials, o => o.MapFrom(s => s.Materials));

        CreateMap<MaintenanceMaterial, MaintenanceMaterialDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : ""));

        CreateMap<CreateMaintenanceLogDto, MaintenanceLog>()
             .ForMember(d => d.Materials, o => o.MapFrom(s => s.UsedMaterials));
        CreateMap<CreateMaintenanceMaterialDto, MaintenanceMaterial>();
        
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

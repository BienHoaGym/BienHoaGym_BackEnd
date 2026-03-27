using AutoMapper;
using Gym.Application.DTOs.Billing;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class BillingProfile : Profile
{
    public BillingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""))
            .ReverseMap();
        CreateMap<CreateProductDto, Product>();

        // Map MembershipPackage to ProductDto for unified POS listing
        CreateMap<MembershipPackage, ProductDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => 1)) // 1: Service (Package)
            .ForMember(d => d.StockQuantity, o => o.MapFrom(s => 999))
            .ForMember(d => d.TrackInventory, o => o.MapFrom(s => false))
            .ForMember(d => d.Unit, o => o.MapFrom(s => "Gói"));

        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : "Khách vãng lai"));

        CreateMap<InvoiceDetail, InvoiceDetailDto>();
        
        CreateMap<CreateInvoiceDto, Invoice>();
        CreateMap<CreateInvoiceDetailDto, InvoiceDetail>();
    }
}

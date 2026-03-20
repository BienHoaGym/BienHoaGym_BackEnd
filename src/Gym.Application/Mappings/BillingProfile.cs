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

        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : "Khách vãng lai"));

        CreateMap<InvoiceDetail, InvoiceDetailDto>();
        
        CreateMap<CreateInvoiceDto, Invoice>();
        CreateMap<CreateInvoiceDetailDto, InvoiceDetail>();
    }
}

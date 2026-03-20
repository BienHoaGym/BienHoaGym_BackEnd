using AutoMapper;
using Gym.Application.DTOs.Subscriptions;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        // Map SubscriptionDto
        CreateMap<MemberSubscription, SubscriptionDto>()
            .ForMember(dest => dest.MemberName,
                opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : "N/A"))
            .ForMember(dest => dest.PackageName,
                opt => opt.MapFrom(src => src.Package != null ? src.Package.Name : "N/A"))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        // Map SubscriptionDetailDto
        CreateMap<MemberSubscription, SubscriptionDetailDto>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        // Map Create
        CreateMap<CreateSubscriptionDto, MemberSubscription>();

        CreateMap<MemberSubscription, SubscriptionDto>()
    .ForMember(dest => dest.MemberName,
        opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : "N/A"))
    .ForMember(dest => dest.PackageName,
        opt => opt.MapFrom(src => src.Package != null ? src.Package.Name : "N/A"))

    // --- THÊM MỚI: Map giá tiền ---
    .ForMember(dest => dest.PackagePrice,
        opt => opt.MapFrom(src => src.Package != null ? src.Package.Price : 0))

    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
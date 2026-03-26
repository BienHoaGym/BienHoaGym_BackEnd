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
                opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : "Khách Hội viên"))
            .ForMember(dest => dest.PackageName,
                opt => opt.MapFrom(src => src.OriginalPackageName))
            .ForMember(dest => dest.PackagePrice,
                opt => opt.MapFrom(src => src.OriginalPrice))
            .ForMember(dest => dest.Price,
                opt => opt.MapFrom(src => src.FinalPrice))
            .ForMember(dest => dest.Discount,
                opt => opt.MapFrom(src => src.DiscountApplied))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        // Map SubscriptionDetailDto
        CreateMap<MemberSubscription, SubscriptionDetailDto>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        // Map Create
        CreateMap<CreateSubscriptionDto, MemberSubscription>();
    }
}
using AutoMapper;
using Gym.Application.DTOs.CheckIns;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class CheckInProfile : Profile
{
    public CheckInProfile()
    {
        CreateMap<CheckIn, CheckInDto>()
            .ForMember(dest => dest.MemberName,
                opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : "N/A"))
            .ForMember(dest => dest.MemberCode,
                opt => opt.MapFrom(src => src.Member != null ? src.Member.MemberCode : "N/A"))
            .ForMember(dest => dest.PackageName,
                opt => opt.MapFrom(src => (src.Subscription != null && src.Subscription.Package != null)
                    ? src.Subscription.Package.Name : "N/A"));

        CreateMap<CreateCheckInDto, CheckIn>();
    }
}

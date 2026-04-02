using AutoMapper;
using Gym.Application.DTOs.Packages;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class PackageProfile : Profile
{
    public PackageProfile()
    {
        CreateMap<MembershipPackage, PackageDto>().ReverseMap();

        CreateMap<CreatePackageDto, MembershipPackage>()
            // S?A L?I ? ÐÂY: Xóa dòng map Status, ch? gi? IsActive
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<UpdatePackageDto, MembershipPackage>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

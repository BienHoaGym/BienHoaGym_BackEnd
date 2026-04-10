using AutoMapper;
using Gym.Application.DTOs.Members;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<Member, MemberDto>().ReverseMap();
        CreateMap<Member, MemberListDto>().ReverseMap();
        CreateMap<CreateMemberDto, Member>();
        CreateMap<UpdateMemberDto, Member>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Không bao giờ cho phép ghi đè ID khi Update
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Notes))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

    private (string First, string Last) SplitFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return (string.Empty, string.Empty);
        var parts = fullName.Trim().Split(' ');
        return (parts[0], parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : string.Empty);
    }
}

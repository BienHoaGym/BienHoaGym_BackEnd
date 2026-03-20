using AutoMapper;
using Gym.Application.DTOs.Classes;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class ClassProfile : Profile
{
    public ClassProfile()
    {
        CreateMap<Class, ClassDto>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.FullName));

        CreateMap<CreateClassDto, Class>();
        CreateMap<UpdateClassDto, Class>();

        CreateMap<ClassEnrollment, ClassAttendanceDto>()
            .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member!.FullName))
            .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.Member!.MemberCode));
    }
}
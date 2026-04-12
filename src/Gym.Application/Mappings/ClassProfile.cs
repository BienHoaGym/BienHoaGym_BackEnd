using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Gym.Application.DTOs.Classes;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class ClassProfile : Profile
{
    public ClassProfile()
    {
        CreateMap<Class, ClassDto>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.FullName))
            .ForMember(dest => dest.ScheduleDay, opt => opt.MapFrom(src => 
                !string.IsNullOrEmpty(src.ScheduleDay) 
                ? src.ScheduleDay.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() 
                : new List<string>()));

        CreateMap<CreateClassDto, Class>()
            .ForMember(dest => dest.ScheduleDay, opt => opt.MapFrom(src => 
                src.ScheduleDay != null ? string.Join(",", src.ScheduleDay) : null));

        CreateMap<UpdateClassDto, Class>()
            .ForMember(dest => dest.ScheduleDay, opt => opt.MapFrom(src => 
                src.ScheduleDay != null ? string.Join(",", src.ScheduleDay) : null));

        CreateMap<ClassEnrollment, ClassAttendanceDto>()
            .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member!.FullName))
            .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.Member!.MemberCode));
    }
}

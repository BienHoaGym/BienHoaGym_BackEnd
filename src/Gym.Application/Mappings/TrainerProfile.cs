using AutoMapper;
using Gym.Application.DTOs.Trainers;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class TrainerProfile : Profile
{
    public TrainerProfile()
    {
        CreateMap<Trainer, TrainerDto>().ReverseMap();
        CreateMap<CreateTrainerDto, Trainer>();
        CreateMap<UpdateTrainerDto, Trainer>();

        CreateMap<TrainerMemberAssignment, TrainerAssignmentDto>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.FullName))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.FullName))
            .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.Member.MemberCode));

        CreateMap<CreateTrainerAssignmentDto, TrainerMemberAssignment>();
    }
}

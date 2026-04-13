using AutoMapper;
using Gym.Application.DTOs.Trainers;
using Gym.Domain.Entities;

namespace Gym.Application.Mappings;

public class TrainerProfile : Profile
{
    public TrainerProfile()
    {
        CreateMap<Trainer, TrainerDto>()
            .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePhoto : null))
            .ReverseMap();
        CreateMap<CreateTrainerDto, Trainer>();
        CreateMap<UpdateTrainerDto, Trainer>();

        CreateMap<TrainerMemberAssignment, TrainerAssignmentDto>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer != null ? src.Trainer.FullName : "Chưa phân công"))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.FullName))
            .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.Member.MemberCode))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.MemberSubscriptionId, opt => opt.MapFrom(src => src.MemberSubscriptionId));

        CreateMap<CreateTrainerAssignmentDto, TrainerMemberAssignment>();
    }
}

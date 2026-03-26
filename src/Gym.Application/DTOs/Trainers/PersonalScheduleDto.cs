using Gym.Application.DTOs.Classes;
using System.Collections.Generic;

namespace Gym.Application.DTOs.Trainers;

public class PersonalScheduleDto
{
    public TrainerDto? UserInfo { get; set; }
    public List<ClassDto> Classes { get; set; } = new();
    public List<TrainerAssignmentDto> PersonalClients { get; set; } = new();
}

using AutoMapper;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.AutoMapper;

public class AutoMapperParticipantProfile: Profile
{
    public AutoMapperParticipantProfile()
    {
        CreateMap<ParticipantForCreationDto, Participant>();
        CreateMap<ParticipantForUpdateDto, Participant>();
        CreateMap<Participant, ParticipantForResponseDto>();
    }
}

using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Models;

namespace EventFoodOrders.AutoMapper
{
    public class AutoMapperParticipantProfile: Profile
    {
        public AutoMapperParticipantProfile()
        {
            CreateMap<ParticipantForCreationDto, Participant>();
            CreateMap<ParticipantForUpdateDto, Participant>();
            CreateMap<Participant, ParticipantForResponseDto>();
        }
    }
}

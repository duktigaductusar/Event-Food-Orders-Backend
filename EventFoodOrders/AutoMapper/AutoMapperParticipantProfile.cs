using AutoMapper;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Entities;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.AutoMapper;

public class AutoMapperParticipantProfile: Profile
{
    public AutoMapperParticipantProfile()
    {
        CreateMap<ParticipantForCreationDto, Participant>()
            .AfterMap((src, dest) => dest.ResponseType = ReType.Pending);

        // Update using extension method MapToParticipantFromUpdateDto to include ResponseType
        CreateMap<ParticipantForUpdateDto, Participant>()
            .ForMember(dest => dest.ResponseType, opt => opt.Ignore());

        CreateMap<Participant, ParticipantForResponseDto>();
    }
}

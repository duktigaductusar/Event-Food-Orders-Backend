using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;

namespace EventFoodOrders.AutoMapper;

public static class AutoMapperExtensions
{
    public static EventForResponseWithDetailsDto MapToEventForResponseDto(this IMapper mapper, Event srcEvent, Participant srcParticipant)
    {
        if (srcEvent.Id != srcParticipant.EventId)
        {
            throw new EventNotFoundException(srcEvent.Id);
        }

        EventForResponseWithDetailsDto dto = mapper.Map<EventForResponseWithDetailsDto>(srcEvent, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.ParticipantID = srcParticipant.Id.ToString();
                dest.IsOwner = srcParticipant.Id == srcEvent.OwnerId;
                dest.ParticipantResponseType = srcParticipant.Response;
                dest.WantsMeal = srcParticipant.WantsMeal;
                dest.Allergies = srcParticipant.Allergies ?? [""];
                dest.Preferences = srcParticipant.Preferences ?? [""];
            })
        );

        return dto;
    }
}

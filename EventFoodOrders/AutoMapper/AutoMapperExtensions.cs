using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using System.Collections.Generic;

namespace EventFoodOrders.AutoMapper;

public static class AutoMapperExtensions
{
    public static EventForResponseDto MapToEventForResponseDto(this IMapper mapper, Event srcEvent, Participant srcParticipant)
    {
        if (srcEvent.Id != srcParticipant.EventId)
        {
            throw new EventNotFoundException(srcEvent.Id);
        }

        EventForResponseDto dto = mapper.Map<EventForResponseDto>(srcEvent, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.IsOwner = srcParticipant.Id == srcEvent.OwnerId;
                dest.ParticipantResponseType = srcParticipant.Response;
            })
        );

        return dto;
    }

    public static IEnumerable<EventForResponseDto> MapEnumerableToEventForResponseDto(this IMapper mapper, IEnumerable<Event> srcEvents, IEnumerable<Participant> srcParticipants)
    {
        IEnumerable<EventForResponseDto> dtos = [];

        if (srcEvents.Count() == srcParticipants.Count())
        {
            
        }
        return [];
    }

    public static EventForResponseWithDetailsDto MapToEventForResponseWithDetailsDto(this IMapper mapper, Event srcEvent, Participant srcParticipant)
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

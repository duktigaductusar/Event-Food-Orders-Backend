using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Entities;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Utilities;
using EventFoodOrders.Dto.UserDTOs;
using System.Collections.ObjectModel;

namespace EventFoodOrders.AutoMapper;

public static class AutoMapperExtensions
{
    public static Event MapToNewEvent(this IMapper mapper, Guid userId, EventForCreationDto eventForCreationDto)
    {
        EventForCreationObject eventCreationObject = mapper.Map<EventForCreationObject>(eventForCreationDto);
        eventCreationObject.OwnerId = userId;
        Event newEvent = mapper.Map<Event>(eventCreationObject);

        return newEvent;
    }

    public static EventForResponseDto MapToEventForResponseDto(this IMapper mapper, Event srcEvent, Participant srcParticipant)
    {
        if (srcEvent.Id != srcParticipant.EventId)
        {
            throw new EventNotFoundException(srcEvent.Id);
        }

        EventForResponseDto dto = mapper.Map<EventForResponseDto>(srcEvent, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.ParticipantId = srcParticipant.Id.ToString();
                dest.IsOwner = srcParticipant.UserId == srcEvent.OwnerId;
                dest.ResponseType = srcParticipant.ResponseType;
            })
        );

        return dto;
    }

    public static IEnumerable<EventForResponseDto> MapEnumerableToEventForResponseDto(this IMapper mapper, IEnumerable<Event> srcEvents, IEnumerable<Participant> srcParticipants)
    {
        List<EventForResponseDto> dtos = [];

        if (srcEvents.Count() == srcParticipants.Count())
        {
            for (int i = 0; i < srcEvents.Count(); i++)
            {
                dtos.Add(mapper.MapToEventForResponseDto(srcEvents.ElementAt(i), srcParticipants.ElementAt(i)));
            }

            return dtos;
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
                dest.ParticipantId = srcParticipant.Id.ToString();
                dest.IsOwner = srcParticipant.UserId == srcEvent.OwnerId;
                dest.ResponseType = srcParticipant.ResponseType;
                dest.WantsMeal = srcParticipant.WantsMeal;
                dest.Allergies = srcParticipant.Allergies ?? "";
                dest.Preferences = srcParticipant.Preferences ?? "";
            }
        ));

        return dto;
    }

    public static EventForResponseWithUsersDto MapToEventForResponseWithUsersDto(this IMapper mapper, EventForResponseWithDetailsDto dto, IEnumerable<ParticipantForResponseDto> participants, IEnumerable<UserDto> users)
    {
        var ps = new Collection<ParticipantWithUserDto>();

        foreach(ParticipantForResponseDto participant in participants)
        {
            ps.Add(mapper.MapToParticipantWithUserDto(participant, users.Where(u => u.UserId == participant.UserId).First()));
        }

        var e = mapper.Map<EventForResponseWithUsersDto>(dto);
        e.Participants = ps;

        return e;
    }

    public static ParticipantWithUserDto MapToParticipantWithUserDto(this IMapper mapper, ParticipantForResponseDto participant, UserDto user)
    {
        var p = mapper.Map<ParticipantWithUserDto>(participant);
        p.UserName = user.Username;
        p.Email = user.Email;
        return p;
    }

    public static Participant MapToParticipantFromCreationDto(this IMapper mapper, Guid eventId, ParticipantForCreationDto participantForCreationDto)
    {
        Participant participant = mapper.Map<Participant>(participantForCreationDto, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.EventId = eventId;
            }
        ));

        return participant;
    }

    public static Participant MapToParticipantFromUpdateDto(this IMapper mapper, Participant participant, ParticipantForUpdateDto participantForUpdateDto)
    {
        participant = mapper.Map(participantForUpdateDto, participant);
        
        // If the input response type is invalid, set it to PENDING
        participant.ResponseType = ReType.Pending;
        
        foreach (string responseType in Utility.PossibleResponses)
        {
            if (participantForUpdateDto.ResponseType == responseType)
            {
                participant.ResponseType = responseType;
                break;
            }
        }

        return participant;
    }
}

using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;

namespace EventFoodOrders.AutoMapper;

public static class AutoMapperExtensions
{
    public static EventForResponseDto MapToEventForResponseDto(this IMapper mapper, Event srcEvent, Participant srcParticipant)
    {
        if (srcEvent.Id != srcParticipant.EventId)
        {
            throw new EventNotFoundException(srcEvent.Id);
        }

        return mapper.Map<EventForResponseDto>(srcEvent, opt =>
        {
            opt.AfterMap((src, dest) =>
            {
                dest.ParticipantID = srcParticipant.Id.ToString();
            });
            opt.AfterMap((src, dest) =>
            {
                dest.IsOwner = srcParticipant.UserId == srcEvent.OwnerId;
            });
            opt.AfterMap((src, dest) =>
            {
                dest.ParticipantResponseType = srcParticipant.Response;
            });
            opt.AfterMap((src, dest) =>
            {
                dest.WantsMeal = srcParticipant.WantsMeal;
            });
            opt.AfterMap((src, dest) =>
            {
                dest.Allergies = srcParticipant.Allergies;
            });
            opt.AfterMap((src, dest) =>
            {
                dest.Preferences = srcParticipant.Preferences;
            });
        });
    }
}

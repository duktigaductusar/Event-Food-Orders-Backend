using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.AutoMapper;

public class AutoMapperEventProfile: Profile
{
    public AutoMapperEventProfile()
    {
        CreateMap<EventForCreationDto, EventForCreationObject>();
        CreateMap<EventForCreationObject, Event>();
        CreateMap<EventForUpdateDto, Event>();
        CreateMap<Event, EventForResponseDto>();
        CreateMap<Event, EventForResponseWithDetailsDto>();
    }
}

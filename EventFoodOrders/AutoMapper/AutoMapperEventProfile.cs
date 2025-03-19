using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Entities;

namespace EventFoodOrders.AutoMapper;

public class AutoMapperEventProfile: Profile
{
    public AutoMapperEventProfile()
    {
        CreateMap<EventForCreationDto, Event>();
        CreateMap<EventForUpdateDto, Event>();
        CreateMap<Event, EventForResponseWithDetailsDto>();
    }
}

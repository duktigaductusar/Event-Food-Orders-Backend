using AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Models;

namespace EventFoodOrders.AutoMapper;

public class AutoMapperEventProfile: Profile
{
    public AutoMapperEventProfile()
    {
        CreateMap<EventForCreationDto, Event>();
        CreateMap<EventForUpdateDto, Event>();
        CreateMap<Event, EventForResponseDto>();
    }

    public void hihi()
    {

        CustomAutoMapper cmapper = new();

        IMapper mapper = cmapper.Mapper;

        Event e = new();

        mapper.Map<EventForResponseDto>(e);
    }
}

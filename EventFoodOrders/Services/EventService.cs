using AutoMapper;
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Dto.EventDTOs;
using EventFoodOrders.Models;
using EventFoodOrders.Repositories;

namespace EventFoodOrders.Services
{
    public class EventService(EventRepository repository, CustomAutoMapper mapper)
    {
        private readonly EventRepository _repository = repository;
        private readonly IMapper _mapper = mapper.Mapper;

        public EventForResponseDTO CreateEvent(EventForCreationDto eventForCreation)
        {
            Event newEvent = _mapper.Map<Event>(eventForCreation);
            newEvent.EventId = Guid.NewGuid();

            newEvent = _repository.CreateEvent(newEvent);

            return _mapper.Map<EventForResponseDTO>(newEvent);
        }

        public EventForResponseDTO UpdateEvent(string eventId, EventForUpdateDto updatedEventDto)
        {
            Event updatedEvent = _mapper.Map<Event>(updatedEventDto);

            updatedEvent = _repository.UpdateEvent(eventId, updatedEvent);

            return _mapper.Map<EventForResponseDTO>(updatedEvent);
        }

        public bool DeleteEvent(string eventId)
        {
            try
            {
                _repository.DeleteEvent(eventId);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public EventForResponseDTO GetEventForUser(string userId, string eventId)
        {
            Event returnEvent = _repository.GetEventForUser(userId, eventId);

            return _mapper.Map<EventForResponseDTO>(returnEvent);
        }

        public IEnumerable<EventForResponseDTO> GetAllEventsForUser(string userId)
        {
            IEnumerable<Event> returnEvents = _repository.GetAllEventsForUser(userId);

            return _mapper.Map<IEnumerable<EventForResponseDTO>>(returnEvents);
        }
    }
}

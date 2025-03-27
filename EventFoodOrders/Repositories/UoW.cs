using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Repositories;

public class UoW(IEventRepository eventRepository, IParticipantRepository participantRepository) : IUoW
{
    public IEventRepository EventRepository { get; set; } = eventRepository;
    public IParticipantRepository ParticipantRepository { get; set; } = participantRepository;
}

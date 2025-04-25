using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Repositories;

public class UoW(
    Lazy<IEventRepository> eventRepository,
    Lazy<IParticipantRepository> participantRepository
) : IUoW
{
    private readonly Lazy<IEventRepository> _eventRepository = eventRepository;
    private readonly Lazy<IParticipantRepository> _participantRepository = participantRepository;

    public IEventRepository EventRepository { get => _eventRepository.Value; }
    public IParticipantRepository ParticipantRepository { get => _participantRepository.Value; }
}

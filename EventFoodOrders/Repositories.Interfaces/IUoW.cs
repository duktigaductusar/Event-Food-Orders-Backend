namespace EventFoodOrders.Repositories.Interfaces;

public interface IUoW
{
    IEventRepository EventRepository { get; }
    IParticipantRepository ParticipantRepository { get; }
}
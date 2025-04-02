namespace EventFoodOrders.Repositories.Interfaces;

public interface IUoW
{
    IEventRepository EventRepository { get; set; }
    IParticipantRepository ParticipantRepository { get; set; }
}
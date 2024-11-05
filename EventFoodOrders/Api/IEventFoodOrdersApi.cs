using EventFoodOrders.Models;

namespace EventFoodOrders.Api;

public interface IEventFoodOrdersApi
{
    public List<Event> GetEvents();
    public List<Participant> GetParticipants();
    public List<User> GetUsers();

    public User GetUser(Guid id);
    public Event GetEvent(Guid id);
    public Participant GetParticipant(Guid id);

    public User createUser(User _user);
    public Event createEvent(Event _event);
    public Participant Participant(Participant _participant);



}

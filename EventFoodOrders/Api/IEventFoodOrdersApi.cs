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
    public Event SaveEvent(Event _event);
    public Participant CreateParticipant(Participant _participant);

    public User UpdateUser(User _user);
    public Event UpdateEvent(Event _event);
    public Participant UpdateParticipant(Participant _participant);

    public List<User> FindByName(string name);

}

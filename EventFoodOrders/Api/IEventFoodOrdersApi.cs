using EventFoodOrders.Models;

namespace EventFoodOrders.Api
{
    public interface IEventFoodOrdersApi
    {
        User? AddUser(User _user);
        Event createEvent(Event _event);
        Participant? CreateParticipant(Participant _participant);
        User createUser(User _user);
        void DeleteEvent(Guid guid);
        void DeleteUser(Guid _guid);
        List<User> FindByName(string _name);
        Event GetEvent(Guid id);
        List<Event> GetEvents();
        List<Participant> getEventWithMeal(Guid _guid);
        Participant GetParticipant(Guid _id);
        Participant GetParticipant(Participant _participant);
        List<Participant> GetParticipants();
        User GetUser(Guid _id);
        List<User> GetUsers();
        Event SaveEvent(Event _event);
        Event UpdateEvent(Event _event);
        Participant UpdateParticipant(Participant _participant);
        User UpdateUser(User _user);
    }
}
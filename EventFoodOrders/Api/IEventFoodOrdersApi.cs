using EventFoodOrders.Dto;
using EventFoodOrders.Models;

namespace EventFoodOrders.Api
{
    public interface IEventFoodOrdersApi
    {
        User? AddUser(User _user);
        Event createEvent(Event _event);
        Participant? CreateParticipant(Participant _participant);
        User createUser(User _user);
        void DeleteEvent(Guid _guid);
        void DeleteUser(Guid _guid);
        List<User> FindByName(string _name);
        Event GetEvent(Guid _id);
        List<Event> GetEvents();
        List<Participant> getEventWithMeal(Guid _guid);
        Participant GetParticipant(Guid id);
        Participant GetParticipant(Participant _participant);
        List<Participant> GetParticipants();
        long getRegistrationsCount(Guid eventId);
        User GetUser(Guid _id);
        List<User> GetUsers();
        LoginResponse Login(UserLoginDTO input);
        Event SaveEvent(Event _event);
        Event SaveEvent(EventDTO _event);
        User signup(User input);
        Event UpdateEvent(string id, Event _event);
        Participant UpdateParticipant(Participant _participant);
        User UpdateUser(string id, User _user);
        User UpdateUser(User _user);
    }
}
using EventFoodOrders.Dto;
using EventFoodOrders.Dto.ParticipantDTOs;
using EventFoodOrders.Models;

namespace EventFoodOrders.Api
{
    public interface IEventFoodOrdersApi
    {
        User? AddUser(User _user);
        void cancelRegistration(Guid _id);
        Event createEvent(Event _event);
        Participant? CreateParticipant(Participant _participant);
        void DeleteEvent(Guid _guid);
        void DeleteUser(Guid _guid);
        Participant? findParticipantByUserIdAndEventId(Guid userId, Guid eventId);
        Event GetEvent(Guid _id);
        List<Event> GetEvents();
        List<Participant> getEventWithMeal(Guid _guid);
        Participant GetParticipant(Guid id);
        Participant GetParticipant(Participant _participant);
        Participant getParticipantDetails(Guid participantId);
        List<Participant> GetParticipants();
        long getRegistrationsCount(Guid eventId);
        User GetUser(Guid _id);
        List<User> GetUsers();
        LoginResponse Login(UserLoginDTO input);
        ParticipantForResponseDto RegisterForEvent(ParticipantForCreationDto _participantRegistrationRequest);
        //Event SaveEvent(EventDTO _event);
        User signup(User input);
        Event UpdateEvent(string id, Event _event);
        Participant UpdateParticipant(Guid id, ParticipantForUpdateDto _participantRegistrationRequest);
        User UpdateUser(string id, User _user);
    }
}
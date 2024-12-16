using EventFoodOrders.Data;
using EventFoodOrders.Dto;
using EventFoodOrders.Exceptions;
using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EventFoodOrders.Api;

public class EventFoodOrdersApi(ILogger<EventFoodOrdersApi> logger, IDbContextFactory<EventFoodOrdersDbContext> contextFactory) : IEventFoodOrdersApi
{
    public IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;
    private readonly ILogger<EventFoodOrdersApi> _logger = logger;

    public List<Event> GetEvents()
    {
        List<Event> Result = [];

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Result = context.Events.ToList();
        }

        return Result;
    }

    public List<Participant> GetParticipants()
    {

        List<Participant> Result = [];

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Result = context.Participants.ToList();
        }

        return Result;
    }

    public List<User> GetUsers()
    {
        List<User> Result = new();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Result = context.Users.ToList();
        }

        return Result;
    }

    public User GetUser(Guid _id)
    {
        User? retVal = null;


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.First(u => u.id == _id);
            if (retVal == null)
            {
                throw new UserNotFoundException("No user for id " + _id.ToString());
            }
        }

        return retVal;
    }

    public Event GetEvent(Guid _id)
    {
        Event retVal = new();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Events.First(e => e.id == _id);
            if (retVal == null)
            {
                throw new EventNotFoundException("No event for id " + _id.ToString());
            }
        }

        return retVal;
    }
    public Participant GetParticipant(Guid id)
    {

        Participant? participant;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            participant = context.Participants.Where(p => p.participant_id == id).FirstOrDefault();
        }

        if (participant == null)
        {
            throw new ParticipantNotFoundException("Participant not found with ID: " + id);
        }

        return participant;
    }

    public Participant UpdateParticipant(Guid id, ParticipantUpdateRequestDTO _participantRegistrationRequest)
    {

        if (_participantRegistrationRequest == null)
        {
            throw new BadRequestException("Can not update participant with null argument");
        }

        Participant existingParticipant = GetParticipant(id);

        if (existingParticipant == null)
        {
            throw new ParticipantNotFoundException("Could not find participant with id: " + id);
        }

        existingParticipant.wantsMeal = _participantRegistrationRequest.wantsMeal;

        if (_participantRegistrationRequest.allergies != null)
        {
            existingParticipant.allergies = _participantRegistrationRequest.allergies;
        }



        Participant? newParticipant = null;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            newParticipant = context.Participants.Update(existingParticipant).Entity;
            context.SaveChanges();
        }

        if (newParticipant == null)
        {
            throw new ParticipantNotFoundException("Failed to update participant");
        }


        User? user = null;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            user = context.Users.Where(u => u.id == newParticipant._user).FirstOrDefault();
        }

        if (user != null)
        {
            user.allergies = _participantRegistrationRequest.allergies == null ? "" : _participantRegistrationRequest.allergies;
            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                user = context.Users.Update(user).Entity;
                context.SaveChanges();
            }
        }
        else
        {

            throw new UserNotFoundException("Failed to update participant");
        }

        Event? newEvent = null;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            newEvent = context.Events.Where(e => e.id == newParticipant._event).FirstOrDefault();
        }

        if (newEvent == null)
        {
            throw new EventNotFoundException("Failed to update participant");
        }

        ParticipantDTO participantDTO = new ParticipantDTO(newParticipant.participant_id, user.id, newEvent.id, new DateTime(newEvent.eventDate.Ticks), newParticipant.wantsMeal, newParticipant.allergies);

        return newParticipant;

    }



    public Event createEvent(Event _event)
    {
        Event retVal = new();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Events.Add(_event).Entity;
            context.SaveChanges();
        }
        return retVal;
    }

    public Participant GetParticipant(Participant _participant)
    {
        Participant retVal = new();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Participants.First(p => p.participant_id == _participant.participant_id);
        }

        return retVal;
    }

    public Participant? CreateParticipant(Participant _participant)
    {
        Participant retVal = new();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Participants.Add(_participant).Entity;
            context.SaveChanges();
        }

        return retVal;
    }

    public User? AddUser(User _user)
    {
        User retVal = new();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.Add(_user).Entity;
            context.SaveChanges();
        }
        return retVal;
    }

    public Event SaveEvent(EventDTO _event)
    {
        Event retVal = new();
        Event toSave = new Event(_event.eventId);
        toSave.description = _event.Description;
        toSave.eventDate = _event.EventDate;
        toSave.eventName = _event.EventName;
        toSave.eventActive = _event.Action;


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Events.Add(toSave).Entity;
            context.SaveChanges();
        }
        return retVal;
    }

    public User UpdateUser(String id, User _user)
    {
        Guid _id;

        try
        {
            _id = new Guid(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("id not a valid guid: " + ex.Message, ex);
            throw new BadRequestException("Wrong format on id: " + id, ex);
        }


        User existingUser = GetUser(_id);

        if (existingUser == null)
        {
            _logger.LogError("Could not find a user with id " + id);
            throw new UserNotFoundException("Could not find a user with id " + id);
        }

        existingUser.allergies = _user.allergies;
        existingUser.Role = _user.Role;
        existingUser.Email = _user.Email;
        existingUser.Name = _user.Name;

        User? retVal = null;


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.Update(existingUser).Entity;
            context.SaveChanges();
        }
        return retVal;
    }

    public void DeleteUser(Guid _guid)
    {

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            User existingUser = context.Users.First(u => u.id == _guid);
            if (existingUser == null)
            {
                throw new UserNotFoundException("No User found with id " + _guid);
            }

            context.Users.Remove(existingUser);
            context.SaveChanges();
        }
    }

    public void DeleteEvent(Guid _guid)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Event existingUser = context.Events.First(e => e.id == _guid);
            if (existingUser == null)
            {
                throw new EventNotFoundException("No Event found with id " + _guid);
            }

            context.Events.Remove(existingUser);
            context.SaveChanges();
        }
    }

    public List<Participant> getEventWithMeal(Guid _guid)
    {
        List<Participant> retVal = new List<Participant>();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Set<Participant>().AsQueryable().Where(p => p._event == _guid).ToList();
        }

        return retVal;
    }

    public Event SaveEvent(Event _event)
    {
        throw new NotImplementedException();
    }
    public User signup(User input)
    {
        if (findByEmail(input.Email).Count > 0)
        {
            throw new EmailAlreadyExistsException("Email already registered: " + input.Email);
        }

        User user = new User();
        user.Email = input.Email;
        user.Name = input.Name;
        user.allergies = input.allergies;
        user.Role = input.Role.ToString();

        User retVal;

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.Add(user).Entity;
            context.SaveChanges();
        }

        return retVal;
    }

    private List<User> findByEmail(String input)
    {
        List<User> retVal = new List<User>();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.Where(p => p.Email == input).ToList();
        }

        return retVal;
    }

    public LoginResponse Login(UserLoginDTO input)
    {
        if (input.email.IsNullOrEmpty())
        {
            throw new UserNotFoundException("input email is null or empty");
        }

        User? user = findByEmail(input.email).FirstOrDefault();
        if (user == null)
        {
            throw new UserNotFoundException("User not found with email: " + input.email);
        }

        if (user.Role != Role.ADMIN.ToString())
        {
            throw new AccessDeniedException("Access denied. Only admins can log in.");
        }

        return new LoginResponse(input.email, "");
    }

    public long getRegistrationsCount(Guid eventId)
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return context.Set<Participant>().AsQueryable().Where(p => p._event == eventId).ToList().Count;
        }
    }

    public Event UpdateEvent(string id, Event _event)
    {
        Event retVal;

        Guid _id;

        try
        {
            _id = new Guid(id);
        }
        catch (Exception ex)
        {
            throw new Exception("id is not a in a correct formated guid", ex);
        }


        Event existingEvent = GetEvent(_id);
        existingEvent.eventActive = _event.eventActive;
        existingEvent.eventDate = _event.eventDate;
        existingEvent.eventName = _event.eventName;
        existingEvent.description = _event.description;


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Events.Update(existingEvent).Entity;
            context.SaveChanges();
            return retVal;

        }
    }

    internal List<Event> GetAvailableEvents()
    {
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            List<Event> retVal = context.Events.Where(e => e.eventActive).ToList();

            return retVal;
        }
    }

    public void cancelRegistration(Guid _id)
    {

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Participant existingRegistration = context.Participants.First(p => p.participant_id == _id);
            if (existingRegistration == null)
            {
                throw new ParticipantNotFoundException("No User found with id " + _id);
            }
            context.Participants.Remove(existingRegistration);
            context.SaveChanges();
        }
    }

    public User UpdateUser(User _user)
    {
        throw new NotImplementedException();
    }

    public Participant? findParticipantByUserIdAndEventId(Guid userId, Guid eventId)
    {
        if (userId == Guid.Empty || eventId == Guid.Empty)
        {
            throw new BadRequestException("Failed to Participand with null values");
        }


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            return context.Participants.Where(p => p._user == userId && p._event == eventId).FirstOrDefault();
        }
    }

    public ParticipantDTO RegisterForEvent(ParticipantRegistrationRequestDTO _participantRegistrationRequest)
    {

        if (_participantRegistrationRequest == null)
        {
            throw new BadRequestException("Parameter can not be null");
        }

        User _user = GetUser(_participantRegistrationRequest.userId);
        if (_user == null)
        {
            throw new BadRequestException("User not found with ID: " + _participantRegistrationRequest.userId.ToString());
        }

        Event _event = GetEvent(_participantRegistrationRequest.eventId);
        if (_event == null)
        {
            throw new BadRequestException("Event not found with ID: " + _participantRegistrationRequest.eventId.ToString());
        }

        var _participant = findParticipantByUserIdAndEventId(_user.id, _event.id);
        if (_participant != null)
        {
            throw new ArgumentException("User is already registered for this event.");
        }

        Participant participant = new Participant();
        participant._user = _user.id;
        participant._event = _event.id;
        participant.wantsMeal = _participantRegistrationRequest.wantsMeal;
        participant.allergies = _user.allergies;

        Participant returnParticipant;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            returnParticipant = context.Participants.Add(participant).Entity;
            context.SaveChanges();
        }

        DateTime dateTime = new DateTime(_event.eventDate.Ticks);

        ParticipantDTO retVal = new ParticipantDTO(returnParticipant.participant_id, _user.id, _event.id, dateTime, _participantRegistrationRequest.wantsMeal, _user.allergies);

        return retVal;
    }

    public Participant getParticipantDetails(Guid participantId)
    {
        Participant? participant;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            participant = context.Participants.Where(p => p.participant_id == participantId).FirstOrDefault();
        }

        if (participant == null)
        {
            throw new ParticipantNotFoundException("Participant not found with ID: " + participantId);
        }

        return participant;

    }

}





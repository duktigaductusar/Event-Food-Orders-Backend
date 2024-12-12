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
        throw new NotImplementedException();
    }

    public User createUser(User _user)
    {
        throw new NotImplementedException();
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
        // TODO Map between EventDTO to Event
        toSave.description = _event.Description;
        toSave.EventDate = _event.EventDate;
        toSave.EventName = _event.EventName;
        toSave.Active = _event.Action;


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

    public Participant UpdateParticipant(Participant _participant)
    {
        throw new NotImplementedException();
    }

    public List<User> FindByName(string _name)
    {
        throw new NotImplementedException();
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
            retVal = context.Participants.Where(p => p._wantsMeal && p.participant_id == _guid).ToList();
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
            return context.Participants.Where(p => p._event == eventId).ToList().Count;
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
        existingEvent.Active = _event.Active;
        existingEvent.EventDate = _event.EventDate;
        existingEvent.EventName = _event.EventName;
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
            List<Event> retVal = context.Events.Where(e => e.Active).ToList();

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

    public Participant findParticipantByUserIdAndEventId(Guid userId, Guid eventId)
    {
        if (userId == null || eventId == null)
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
        participant._wantsMeal = _participantRegistrationRequest.wantsMeal;
        participant._allergies = _user.allergies;

        Participant returnParticipant;
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            returnParticipant = context.Participants.Add(participant).Entity;
            context.SaveChanges();
        }

        DateTime dateTime = new DateTime(_event.EventDate.Ticks);

        ParticipantDTO retVal = new ParticipantDTO(returnParticipant.participant_id, _user.id, _event.id, dateTime, _participantRegistrationRequest.wantsMeal, _user.allergies);

        return retVal;
    }

    public ParticipantDTO getParticipantDetails(Guid participantId)
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

        Event tmpEvent = GetEvent(participant._event);

        return new ParticipantDTO(
                participant.participant_id,
                participant._user,
                participant._event,
                new DateTime(tmpEvent.EventDate.Ticks),
                participant._wantsMeal,
                participant._allergies
        );
    }
}





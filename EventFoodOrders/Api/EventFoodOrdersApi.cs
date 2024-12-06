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
        List<Event> Result = new();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            Result = context.Events.ToList();
        }

        return Result;
    }

    public List<Participant> GetParticipants()
    {

        List<Participant> Result = new();

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
        User retVal = new();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            User tmp = context.Users.Find(_id)!;
            if (tmp != null)
            {
                retVal = tmp;
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
        Participant retVal = new Participant();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Participants.Add(_participant).Entity;
            context.SaveChanges();
        }

        return retVal;
    }

    public User? AddUser(User _user)
    {
        User retVal = new User();

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
        Event toSave = new Event(_event.getEventId());
        // TODO Map between EventDTO to Event


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
            //TODO: Create own custom exception and thorw that exception
            _logger.LogError("id not a valid guid: " + ex.Message, ex);
            throw;
        }


        User existingUser = GetUser(_id);

        if (existingUser == null)
        {
            _logger.LogError("Could not find a user with id " + id);
            throw new Exception("Could not find a user with id " + id);
        }

        existingUser.allergies = _user.allergies;
        existingUser.Role = _user.Role;
        existingUser.Email = _user.Email;
        existingUser.Name = _user.Name;

        User retVal = new();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.Update(existingUser).Entity;
            context.SaveChanges();
        }
        return retVal;
    }

    public Event UpdateEvent(Event _event)
    {
        throw new NotImplementedException();
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
            retVal = context.Participants.Where(p => p.wantsMeal && p.participant_id == _guid).ToList();
        }

        return retVal;
    }

    public Event SaveEvent(Event _event)
    {
        throw new NotImplementedException();
    }

    public User UpdateUser(User _user)
    {
        throw new NotImplementedException();
    }


    public User signup(UserDTO input)
    {
        if (findByEmail(input.Email).Count > 0)
        {
            throw new EmailAlreadyExistsException("Email already registered: " + input.Email);
        }

        User user = new User();
        user.Email = input.Email;
        user.Name = input.Name;
        user.allergies = input.Allergies;
        user.Role = input.Role.ToString();

        User retVal;

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Users.Add(user).Entity;
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

        User user = findByEmail(input.email).FirstOrDefault();
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
            return context.Participants.Where(p => p._event.id == eventId).ToList().Count;
        }

    }
}

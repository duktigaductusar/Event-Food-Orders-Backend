using EventFoodOrders.Data;
using EventFoodOrders.Dto;
using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Api;

public class EventFoodOrdersApi : IEventFoodOrdersApi
{
    public IDbContextFactory<EventFoodOrdersDbContext> _contextFactory;

    public EventFoodOrdersApi(IDbContextFactory<EventFoodOrdersDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

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
            retVal = context.Participants.First(p => p.id == _participant.id);
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

    public User UpdateUser(User _user)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void DeleteEvent(Guid _guid)
    {
        throw new NotImplementedException();
    }

    public List<Participant> getEventWithMeal(Guid _guid)
    {
        List<Participant> retVal = new List<Participant>();


        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Participants.Where(p => p.wantsMeal && p.id == _guid).ToList();
        }

        return retVal;
    }

    public Event SaveEvent(Event _event)
    {
        throw new NotImplementedException();
    }
}

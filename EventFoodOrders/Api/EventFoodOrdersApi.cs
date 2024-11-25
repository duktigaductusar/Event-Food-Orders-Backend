using EventFoodOrders.Data;
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public User GetUser(Guid _id)
    {
        throw new NotImplementedException();
    }

    public Event GetEvent(Guid _id)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public Participant GetParticipant(Participant _participant)
    {
        throw new NotImplementedException();
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
            retVal = context.Users.Add(user).Entity;
            context.SaveChanges();
        }
        return retVal;
    }

    public Event SaveEvent(Event _event)
    {
        Event retVal = new Event();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Events.Add(_event).Entity;
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
        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            //    context.Participants.Where((Participant p) =>
            //    {
            //        p.
            //    })
            //        context.SaveChanges();
            //}

            throw new NotImplementedException();
        }
    }
}

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


    public List<Dummy> GetDummies()
    {
        List<Dummy> result = new List<Dummy>();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            result = context.Dummies.ToList();
        }

        return result;

    }
    public Dummy GetDummy(string id)
    {
        Dummy result = new Dummy();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            result = context.Dummies.Where(d => d.Id == id).FirstOrDefault();
        }

        return result;

    }

    public void DeleteDummy(string id)
    {

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            context.Dummies.Remove(GetDummy(id));
            context.SaveChanges();
        }

    }



    public Dummy AddDummy(Dummy item)
    {
        Dummy retVal = new Dummy();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {
            retVal = context.Dummies.Add(item).Entity;
            context.SaveChanges();
        }
        return retVal;


    }
    public Dummy? UpdateDummy(Dummy item)
    {
        Dummy retVal = new Dummy();

        using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
        {

            try
            {
                Dummy existing = context.Dummies.Where(d => d.Id == item.Id).FirstOrDefault();

                if (existing != null)
                {
                    existing.Description = item.Description;
                    existing.Name = item.Name;

                    retVal = context.Dummies.Update(existing).Entity;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return null;
            }



        }
        return retVal;


    }

    public List<Event> GetEvents()
    {
        throw new NotImplementedException();
    }

    public List<Participant> GetParticipants()
    {
        throw new NotImplementedException();
    }

    public List<User> GetUsers()
    {
        throw new NotImplementedException();
    }

    public User GetUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public Event GetEvent(Guid id)
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

    public Participant Participant(Participant _participant)
    {
        throw new NotImplementedException();
    }
}

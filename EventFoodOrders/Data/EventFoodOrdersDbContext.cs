using EventFoodOrders.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Data;

public class EventFoodOrdersDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }

    public EventFoodOrdersDbContext(DbContextOptions options) : base(options)
    { }
}

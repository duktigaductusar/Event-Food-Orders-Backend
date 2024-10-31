using EventFoodOrders.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Data;

public class EventFoodOrdersDbContext : DbContext
{
    public EventFoodOrdersDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Dummy> Dummies { get; set; }

}

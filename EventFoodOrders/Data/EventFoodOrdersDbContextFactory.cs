using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Data
{
    public class EventFoodOrdersDbContextFactory : IDbContextFactory<EventFoodOrdersDbContext>
    {
        public EventFoodOrdersDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<EventFoodOrdersDbContext>();
            //ServiceCollection.
            //    return new EventDataContext(builder.Options);
            return null;
        }
    }
}

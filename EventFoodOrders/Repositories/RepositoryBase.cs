using EventFoodOrders.Data;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories
{
    public class RepositoryBase<T>(IDbContextFactory<EventFoodOrdersDbContext> contextFactory) where T : class
    {
        private readonly IDbContextFactory<EventFoodOrdersDbContext> _contextFactory = contextFactory;

        internal T GetSingle(Func<T, bool> condition)
        {
            T? result;

            using (EventFoodOrdersDbContext context = _contextFactory.CreateDbContext())
            {
                result = context.Find<T>(condition);
            }

            if (result is null)
            {
                throw new ArgumentException("Instance not found.");
            }

            return result;
        }
    }
}

using EventFoodOrders.Data;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories
{
    public class RepositoryBase<T> where T : class
    {
        internal static T GetSingleWithCondition(DbSet<T> dbSet, Func<T, bool> condition)
        {
            T? result;

            result = dbSet.Where(condition).FirstOrDefault();

            if (result is null)
            {
                throw new ArgumentException("Instance not found.");
            }

            return result;
        }

        internal static IEnumerable<T> GetAllWithCondition(DbSet<T> dbSet, Func<T, bool> condition)
        {
            return [.. dbSet.Where(condition)]; ;
        }
    }
}

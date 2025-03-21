using EventFoodOrders.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Repositories;

public class RepositoryBase<T, Q>()
    where T : class
    where Q : Exception, new()
{
    private readonly ExceptionStandIn<Q> _exception = new();

    internal T GetSingleWithCondition(DbSet<T> dbSet, Func<T, bool> condition)
    {
        T? result;

        result = dbSet.Where(condition).FirstOrDefault();

        if (result is null)
        {
            _exception.ThrowDefaultException();
        }

        return result!;
    }

    internal static IEnumerable<T> GetAllWithCondition(DbSet<T> dbSet, Func<T, bool> condition)
    {
        return [.. dbSet.Where(condition)]; ;
    }
}

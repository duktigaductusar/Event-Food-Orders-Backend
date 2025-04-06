using EventFoodOrders.Data;
using EventFoodOrders.Mock;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Extensions;

public static class DBExtension
{
    public static void UseDbExtension(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<EventFoodOrdersDbContext>();
        context.Database.Migrate();
        var userSeed = serviceProvider.GetRequiredService<IUserSeed>();

        Console.WriteLine("Seeding data...");
        DBSeed.Run(context, userSeed);
    }
}

using EventFoodOrders.Data;
using EventFoodOrders.Mock;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Extensions;

public static class DBExtension
{
    public static void UseDBExtension(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<EventFoodOrdersDbContext>();
        var userSeed = serviceProvider.GetRequiredService<IUserSeed>();

        context.Database.Migrate();

        Console.WriteLine("Seeding data...");
        DBSeed.Run(context, userSeed);
    }
}

using EventFoodOrders.Data;

namespace EventFoodOrders.Extensions;

public static class SeedExtension
{
    public static void UseDataSeedExtension(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<EventFoodOrdersDbContext>();

        Console.WriteLine("Seeding data...");
        DBSeed.Run(context);
    }
}

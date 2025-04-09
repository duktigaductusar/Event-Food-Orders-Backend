using EventFoodOrders.Data;
using EventFoodOrders.Mock;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders.Extensions;

public static class DBExtension
{
    public static void ConfigureDatabaseExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<EventFoodOrdersDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DbContext")));
    }

    public static void UseDatabaseExtension(this IApplicationBuilder app, bool isDevelopment)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<EventFoodOrdersDbContext>();
        context.Database.Migrate();        

        if (isDevelopment)
        {
            var userSeed = serviceProvider.GetRequiredService<IUserSeed>();
            Console.WriteLine("Seeding data...");
            DBSeed.Run(context, userSeed);
        }
    }
}

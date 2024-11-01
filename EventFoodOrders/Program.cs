using EventFoodOrders.Api;
using EventFoodOrders.Data;
using Microsoft.EntityFrameworkCore;

namespace EventFoodOrders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContextFactory<EventFoodOrdersDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
        builder.Services.AddTransient<IEventFoodOrdersApi, EventFoodOrdersApi>();

        // Adjust for prod.
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AngularFontendDEV", policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                //policyBuilder.AllowCredentials();
            });
        });

        var app = builder.Build();

        app.UseCors("AngularFontendDEV");

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

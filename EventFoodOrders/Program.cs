using EventFoodOrders.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DotNetEnv;
using EventFoodOrders.Extensions;
using EventFoodOrders.Middleware;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Services;
using EventFoodOrders.Mock;

namespace EventFoodOrders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var isDevelopment = builder.Environment.IsDevelopment();
        
        builder.Services.AddControllers();
        //Auth thingies
        Env.Load();
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddHttpClient<IUserService, UserService>();
        builder.Services.ConfigureScopedServices(isDevelopment);
        builder.Services.ConfigureSingletonServices();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.ConfigureSessions();
        builder.Services.ConfigureAuths(builder.Configuration);
        // No more auth thingies
        builder.Services.AddDbContextFactory<EventFoodOrdersDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
        builder.Services.ConfigureCORS(isDevelopment);
 
        if (isDevelopment)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        var app = builder.Build();

        app.UseCustomExceptionHandler();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseDataSeedExtension();
        
        if (isDevelopment)
        {
            app.UseCors("FrontendDEV");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
        else
        {
            app.UseCors("AngularFontendProd");
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllers();

        app.Run();
    }
}

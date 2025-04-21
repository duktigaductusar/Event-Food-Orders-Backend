using DotNetEnv;
using EventFoodOrders.Extensions;
using EventFoodOrders.Middleware;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Services;
using EventFoodOrders.Options;

namespace EventFoodOrders;

public class Program
{
    public static void Main(string[] args)
    {
        #region Build Phase
        var builder = WebApplication.CreateBuilder(args);
        var isDevelopment = builder.Environment.IsDevelopment();

        Env.Load();
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.ConfigureOptionsExtension(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddHttpClient<IUserService, UserService>();

        builder.Services.ConfigureDatabaseExtension(builder.Configuration);
        builder.Services.ConfigureScopedServices(isDevelopment);
        builder.Services.ConfigureSingletonServices();
        builder.Services.ConfigureHostedServices();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.ConfigureSessions();
        builder.Services.ConfigureAuths(builder.Configuration);
        builder.Services.ConfigureHostedServices();

        builder.Logging.AddConsole();
        builder.Services.ConfigureCORS(isDevelopment);
        builder.Services.ConfigureSwaggerExtension(isDevelopment);

        var app = builder.Build();
        #endregion Build Phase

        #region Pipeline Configuration Phase  
        app.UseCustomExceptionHandler();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseDatabaseExtension(isDevelopment);
        app.UseSwaggerExtension(isDevelopment);
        app.UseCORSExtension(isDevelopment);

        app.UseAuthentication();
        app.UseAuthorization();
        app.UserCustomIdHandler();
        app.UseSession();

        app.MapControllers();

        app.Run();
        #endregion Pipeline Configuration Phase

    }
}

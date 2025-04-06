using EventFoodOrders.AutoMapper;
using EventFoodOrders.Mock;
using EventFoodOrders.Repositories;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Extensions;

public static class ServiceExtension
{
    public static void ConfigureScopedServices(this IServiceCollection services, bool isDev)
    {
        services.AddScoped<IUserSeed, UserSeed>();
        // services.AddScoped<IUserService, MockUserService>();
        // services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserService, MockWithGraphUserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IParticipantService, ParticipantService>();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IUoW, UoW>();
        services.AddScoped<ICustomAutoMapper, CustomAutoMapper>();
    }

    public static void ConfigureSingletonServices(this IServiceCollection services)
    {
        services.AddSingleton<IGraphTokenService, GraphTokenService>();
    }
    
}
using EventFoodOrders.AutoMapper;
using EventFoodOrders.IdHandling;
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
        if (isDev)
        {
            services.AddScoped<IUserSeed, UserSeed>();
            services.AddScoped<IUserService, MockWithGraphUserService>();
        }
        else
        {
            services.AddScoped<IUserService, UserService>();
        }
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IParticipantService, ParticipantService>();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IUoW, UoW>();
        services.AddScoped<ICustomAutoMapper, CustomAutoMapper>();
        services.AddScoped<IMailerService, MailerService>();
        services.AddScoped<IMailManager, MailManager>();
        services.AddScoped<IIdCarrier, CustomIdCarrier>();
    }

    public static void ConfigureSingletonServices(this IServiceCollection services)
    {
        services.AddSingleton<IGraphTokenService, GraphTokenService>();
    }

    public static void ConfigureHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<ReminderService>();
        services.AddHostedService<SummaryService>();
    }
    
}
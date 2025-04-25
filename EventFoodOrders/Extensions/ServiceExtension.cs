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
            services.AddAsLazy<IUserSeed, UserSeed>();
            services.AddAsLazy<IUserService, MockWithGraphUserService>();
        }
        else
        {
            services.AddAsLazy<IUserService, UserService>();
        }

        services.AddAsLazy<IEventService, EventService>();
        services.AddAsLazy<IParticipantService, ParticipantService>();
        services.AddAsLazy<IMailerService, MailerService>();
        services.AddAsLazy<IMailManager, MailManager>();
        services.AddAsLazy<IIdCarrier, CustomIdCarrier>();
        services.AddScoped<IServiceManager, ServiceManager>(); 

        services.AddAsLazy<IEventRepository, EventRepository>();
        services.AddAsLazy<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IUoW, UoW>();

        services.AddScoped<ICustomAutoMapper, CustomAutoMapper>();
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

    private static void AddAsLazy<IServiceType, ServiceType>(
        this IServiceCollection collection,
        ServiceLifetime lifetime = ServiceLifetime.Scoped
    )
        where ServiceType : class, IServiceType
        where IServiceType : class
    {
        collection.Add(new ServiceDescriptor(
            typeof(IServiceType),
            typeof(ServiceType),
            lifetime
        ));

        collection.Add(new ServiceDescriptor(
            typeof(Lazy<IServiceType>),
            p => new Lazy<IServiceType>(() => p.GetRequiredService<IServiceType>()),
            lifetime
        ));
    }
}
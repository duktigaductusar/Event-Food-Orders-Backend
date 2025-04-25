using EventFoodOrders.Options;

namespace EventFoodOrders.Extensions;

public static class OptionsExtension
{
    public static void ConfigureOptionsExtension(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptions<EventFoodOrdersOptions>()
            .Bind(configuration.GetSection("EventFoodOrders"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<AppGraphOptions>()
            .Bind(configuration.GetSection("Graph"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}

namespace EventFoodOrders.Extensions;

public static class CORSExtension
{
    private static readonly string devPolicy = "devPolicy";

    private static readonly string prodPolicy = "prodPolicy";

    public static void ConfigureCORS(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(devPolicy, policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
        else
        {
            services.AddCors(options =>
            {
                options.AddPolicy(prodPolicy, policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }

    public static void UseCORSExtension(this WebApplication app, bool isDevelopment) 
    {
        if (isDevelopment)
        {
            app.UseCors(devPolicy);
        }
        else
        {
            app.UseCors(prodPolicy);
        }
    }
}

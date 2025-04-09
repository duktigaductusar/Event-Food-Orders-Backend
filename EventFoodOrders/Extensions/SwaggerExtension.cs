using Microsoft.OpenApi.Models;

namespace EventFoodOrders.Extensions;

public static class SwaggerExtension
{
    private static readonly string apiTitle = "Event Food Orders API V1";
    private static readonly string apiVersion = "v1";

    public static void ConfigureSwaggerExtension(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(apiVersion, new OpenApiInfo {
                    Title = apiTitle,
                    Version = apiVersion
                });
            });
        }
    }

    public static void UseSwaggerExtension(this WebApplication app, bool isDevelopment)
    {
        if (isDevelopment)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", apiTitle);
            });
        }
    }
}

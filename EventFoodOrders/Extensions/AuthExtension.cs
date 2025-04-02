using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EventFoodOrders.Extensions;

public static class AuthExtension
{
    public static void ConfigureSessions(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    public static void ConfigureAuths(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://login.microsoftonline.com/{configuration["AzureAd:TenantId"]}/v2.0";
                options.Audience = configuration["AzureAd:ClientId"];
            });
        services.AddAuthorization();
    }

    public static void ConfigureCORS(this IServiceCollection services, bool isDev)
    {
        if (isDev)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendDEV", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    // policyBuilder.AllowCredentials();

                });
            });
        }
        else
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AngularFontendProd", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin();
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    //policyBuilder.AllowCredentials();
                });
            });
        }
    }
}
using EventFoodOrders.AutoMapper;
using EventFoodOrders.Data;
using EventFoodOrders.Repositories;
using EventFoodOrders.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DotNetEnv;
using EventFoodOrders.security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventFoodOrders.Extensions;
using EventFoodOrders.Middleware;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var isDevelopment = builder.Environment.IsDevelopment();

        if (isDevelopment)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }
        
        builder.Services.AddControllers();

        //Auth thingies
        Env.Load();
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddHttpClient();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<IParticipantService, ParticipantService>();
        builder.Services.AddScoped<IServiceManager, ServiceManager>();

        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
        builder.Services.AddScoped<IUoW, UoW>();

        builder.Services.AddScoped<ICustomAutoMapper, CustomAutoMapper>();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/v2.0";
                options.Audience = builder.Configuration["AzureAd:ClientId"];
            });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IJwtUtility, JwtUtility>();
        // No more auth thingies
        
        builder.Services.AddDbContextFactory<EventFoodOrdersDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));



        if (isDevelopment)
        {
            builder.Services.AddCors(options =>
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

            builder.Services.AddCors(options =>
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

        var app = builder.Build();

        app.UseCustomExceptionHandler();

        // Error handling
        // Hsts for security
        if (isDevelopment)
        {
            //app.UseDeveloperExceptionPage();
        }
        else
        {
            //app.UseExceptionHandler();
            //app.UseHsts();
        }

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

using EventFoodOrders.AutoMapper;
using EventFoodOrders.Data;
using EventFoodOrders.Interfaces;
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
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect("AzureAd", options =>
            {
                options.Authority =
                    $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/v2.0";
                options.ClientId = builder.Configuration["AzureAd:ClientId"];
                options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
                options.ResponseType = "code";
                options.SaveTokens = false;
                options.UseTokenLifetime = true;
                options.CallbackPath = "/signing-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("https://graph.microsoft.com/Mail.Send");
            })
            .AddJwtBearer("Jwt", options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("jwt_token", out var jwtToken))
                        {
                            context.Token = jwtToken;
                        }

                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
                };
            });
        builder.Services.AddScoped<IJwtUtility, JwtUtility>();
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<IAccessTokenStore, InMemoryAccessTokenStore>();
        // No more auth thingies
        
        builder.Services.AddDbContextFactory<EventFoodOrdersDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));

        builder.Services.AddTransient<CustomAutoMapper>();
        builder.Services.AddTransient<EventService>();
        builder.Services.AddTransient<EventRepository>();
        builder.Services.AddTransient<ParticipantService>();
        builder.Services.AddTransient<ParticipantRepository>();


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

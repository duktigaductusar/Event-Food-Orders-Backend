using EventFoodOrders.AutoMapper;
using EventFoodOrders.Data;
using EventFoodOrders.Repositories;
using EventFoodOrders.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
                options.AddPolicy("AngularFontendDEV", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin();
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    //policyBuilder.AllowCredentials();
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

        if (isDevelopment)
        {
            app.UseCors("AngularFontendDEV");
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


        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        if (isDevelopment)
        {
            app.UsePathBase("/efobackend");
        }
        else
        {
            app.UsePathBase("");
        }

        app.MapControllers();

        app.Run();
    }
}

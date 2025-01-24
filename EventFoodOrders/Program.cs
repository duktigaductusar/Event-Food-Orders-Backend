using EventFoodOrders.Api;
using EventFoodOrders.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EventFoodOrders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });


        builder.Services.AddControllers();
        builder.Services.AddDbContextFactory<EventFoodOrdersDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("EventFoodOrdersProdServer")));
        builder.Services.AddTransient<IEventFoodOrdersApi, EventFoodOrdersApi>();

        //TODO Adjust for prod.
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


        var app = builder.Build();

        app.UseCors("AngularFontendDEV");
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UsePathBase("/efobackend");

        app.MapControllers();

        app.Run();
    }
}

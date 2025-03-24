using System.Globalization;
using System.Runtime.CompilerServices;
using EventFoodOrders.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventFoodOrders.Middleware;

public class CustomExceptionHandler(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is CustomException cEx)
            {
                context.Response.StatusCode = cEx.Code;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
    }
}

public static class CustomExceptionHandlerExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandler>();
    }
}

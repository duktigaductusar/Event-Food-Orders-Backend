using System.Text.Json;
using EventFoodOrders.Exceptions;

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
            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "An unexpected error occurred.";

            if (ex is CustomException cEx)
            {
                statusCode = cEx.Code;
                message = cEx.Message;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                status = statusCode,
                error = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
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

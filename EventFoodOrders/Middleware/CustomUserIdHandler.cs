using EventFoodOrders.Exceptions;
using EventFoodOrders.IdHandling;

namespace EventFoodOrders.Middleware;

public class CustomUserIdHandler(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IIdCarrier carrier)
    {
        string? userIdAsString = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        if (userIdAsString is null || Guid.TryParse(userIdAsString, out Guid userId) == false)
        {
            throw new UnauthorizedUserException();
        }
        carrier.UserId = userId;
        await _next(context);
        carrier.UserId = Guid.NewGuid();
    }
}

public static class CustomUserIdHandlerExtension
{
    public static IApplicationBuilder UserCustomIdHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomUserIdHandler>();
    }
}

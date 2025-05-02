using EventFoodOrders.Exceptions;

public class RequestSizeLimitMiddleware
{
    private readonly RequestDelegate _next;
    private const long MaxBytes = 400 * 1024; // 400 KB

    public RequestSizeLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        var buffer = new byte[MaxBytes + 1];
        var read = await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
        if (read > MaxBytes)
        {
            throw new CustomException(StatusCodes.Status413PayloadTooLarge, "Förfrågan är för stor.");
        }

        context.Request.Body.Position = 0;
        await _next(context);
    }
}

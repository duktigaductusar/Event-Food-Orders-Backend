namespace EventFoodOrders.Services;

public interface IGraphTokenService
{
    Task<string> GetAccessToken();
}
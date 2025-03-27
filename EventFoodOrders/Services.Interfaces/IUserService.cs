namespace EventFoodOrders.Services.Interfaces
{
    public interface IUserService
    {
        string GetName(Guid userId);
        List<string> GetNames(List<Guid> userIds);
        void SendEmail(List<Guid> userIds, string message);
    }
}
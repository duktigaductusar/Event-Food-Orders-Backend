using EventFoodOrders.Interfaces;

namespace EventFoodOrders.Services.Interfaces
{
    public interface IServiceManager
    {
        EventFoodOrders.Interfaces.IAuthService AuthorizationService { get; set; }
        IEventService EventService { get; set; }
        IParticipantService ParticipantService { get; set; }
    }
}
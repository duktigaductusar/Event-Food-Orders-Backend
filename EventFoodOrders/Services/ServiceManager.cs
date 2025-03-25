using EventFoodOrders.Interfaces;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services
{
    public class ServiceManager : IServiceManager
    {
        public IEventService EventService { get; set; }
        public IParticipantService ParticipantService { get; set; }
        public EventFoodOrders.Interfaces.IAuthService AuthService { get; set; }
    }
}

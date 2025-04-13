using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

/**
 * Todo implement Lazy and ass mail service and manager here.
 *  1) Remove circular dependency when using services in services.
 *  2) Reduce memory usage.
 *  3) Improve performance.
 */
public class ServiceManager(
    IEventService eventService,
    IParticipantService participantService,
    IUserService userService
) : IServiceManager
{
    public IEventService EventService { get; set; } = eventService;
    public IParticipantService ParticipantService { get; set; } = participantService;
    public IUserService UserService { get; set; } = userService;
}

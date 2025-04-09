namespace EventFoodOrders.Services.Interfaces;

public interface IServiceManager
{
    IEventService EventService { get; set; }
    IParticipantService ParticipantService { get; set; }
    IUserService UserService { get; set; }
}
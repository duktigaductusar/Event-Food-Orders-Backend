namespace EventFoodOrders.Services.Interfaces;

public interface IServiceManager
{
    IAuthService AuthService { get; set; }
    IEventService EventService { get; set; }
    IParticipantService ParticipantService { get; set; }
    IUserService UserService { get; set; }
}
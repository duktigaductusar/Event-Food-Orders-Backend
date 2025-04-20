using EventFoodOrders.IdHandling;

namespace EventFoodOrders.Services.Interfaces;

public interface IServiceManager
{
    IEventService EventService { get; }
    IParticipantService ParticipantService { get; }
    IUserService UserService { get; }
    IMailManager MailManager { get; }
    IMailerService MailerService{ get; }
    IIdCarrier IdCarrier { get; }
}
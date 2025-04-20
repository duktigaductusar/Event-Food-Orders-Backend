using EventFoodOrders.IdHandling;
using EventFoodOrders.Services.Interfaces;

namespace EventFoodOrders.Services;

public class ServiceManager(
    Lazy<IEventService> eventService,
    Lazy<IParticipantService> participantService,
    Lazy<IUserService> userService,
    Lazy<IMailManager> mailManager,
    Lazy<IMailerService> mailerService,
    Lazy<IIdCarrier> idCarrier
) : IServiceManager
{
    private readonly Lazy<IEventService> _eventService = eventService;
    private readonly Lazy<IParticipantService> _participantService = participantService;
    private readonly Lazy<IUserService> _userService = userService;
    private readonly Lazy<IMailManager> _mailManager = mailManager;
    private readonly Lazy<IMailerService> _mailerService = mailerService;
    private readonly Lazy<IIdCarrier> _idCarrier = idCarrier;

    public IEventService EventService { get => _eventService.Value; }
    public IParticipantService ParticipantService { get => _participantService.Value; }
    public IUserService UserService { get => _userService.Value; }
    public IMailManager MailManager { get => _mailManager.Value; }
    public IMailerService MailerService { get => _mailerService.Value; }
    public IIdCarrier IdCarrier { get => _idCarrier.Value; }
}

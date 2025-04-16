using EventFoodOrders.Entities;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

// TODO: Replace Timer with Task.Delay + CancellationToken for cleaner async handling and proper shutdown support
// Read backend section at https://dataductus.atlassian.net/wiki/spaces/EFO/pages/3468951557/ToDo+s+som+r+kvar.
public class ReminderService(ILogger<ReminderService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private Timer? _timer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.Now;
        var nextRunTime = DateTime.Today.AddHours(7).AddMinutes(30); //Run service at 7:30 every morning.
        if (now > nextRunTime)
        {
            nextRunTime = nextRunTime.AddDays(1);
        }

        var initialDelay = nextRunTime - now;
        
        logger.LogInformation($"Reminder service will start in {initialDelay.TotalSeconds} seconds.");

        _timer = new Timer(async void (state) =>
        {
            try
            {
                await DoWork(state);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }, null, initialDelay, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async Task DoWork(object? state)
    {
        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        {
            return;
        }
        List<DateTime> daysToCheck = [DateTime.Now.AddDays(1)]; //ToDo: Hardcoded 1 workday before deadline reminders, could be dynamic
        if (daysToCheck[0].DayOfWeek == DayOfWeek.Saturday)
        {
            daysToCheck.Add(DateTime.Now.AddDays(2));
            daysToCheck.Add(DateTime.Now.AddDays(3));
        }
        
        logger.LogInformation("Reminder service started at: {time}", DateTimeOffset.Now);
        using (var scope = scopeFactory.CreateScope())
        {
            var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
            var mailerService = scope.ServiceProvider.GetRequiredService<IMailerService>();

            List<Event> reminderEvents = [];
            foreach (var dateTime in daysToCheck)
            {
                reminderEvents.AddRange(await uow.EventRepository.GetAllEventsAtDeadline(dateTime));
            }
            
            if (reminderEvents.Count > 0)
            {
                foreach (var item in reminderEvents)
                {
                    var participants = item.Participants
                        .Where(p => p.ResponseType == ReType.Pending)
                        .Select(p  => p.UserId)
                        .ToList();
                    participants.Add(item.OwnerId);
                    if (participants.Count <= 0) continue;
                    
                    // TODO! Uncomment this method
                    // The mocked service will write to file during development instead of sending mails, this method can therefor be called in development.
                    // await mailerService.SendReminderMail(participants, item);
                    
                    logger.LogInformation("Reminder service running for participant list for event: " + item.Title);
                }
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Reminder service stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }
}
using EventFoodOrders.Entities;
using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class ReminderService(
    ILogger<ReminderService> logger,
    IServiceScopeFactory scopeFactory
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ReminderService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Use Swedish now to schedule reminders mail.  
            var now = DateUtility.GetSwedishDateTimeOffsetNow().DateTime;
            var delay = GetDelayToNextRun(now);

            try
            {
                logger.LogInformation("ReminderService will wait {Delay} before running (next run at {NextRun}).", delay, now.Add(delay));
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("ReminderService is stopping due to cancellation.");
                break;
            }

            try
            {
                await DoEventReminderWork(now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReminderService encountered an error.");
            }
        }

        logger.LogInformation("ReminderService stopped.");
    }

    private static TimeSpan GetDelayToNextRun(DateTimeOffset now)
    {
        // 06:30 Swedish time today.
        var nextRunTime = now.Date.AddHours(12).AddMinutes(27);

        if (now > nextRunTime)
        {
            // Schedule for tomorrow.
            nextRunTime = nextRunTime.AddDays(1);
        }

        return nextRunTime - now;
    }

    private async Task DoEventReminderWork(DateTimeOffset now)
    {
        logger.LogInformation("ReminderService running at: {Time}", now);

        var daysToCheck = GetDaysToCheckForDeadline(now);

        using var scope = scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
        var mailerService = scope.ServiceProvider.GetRequiredService<IMailerService>();

        var reminderEvents = new List<Event>();

        foreach (var date in daysToCheck)
        {
            // Convert Swedish date back to universal time to check
            // against stored values in database.
            reminderEvents.AddRange(
                await uow.EventRepository.GetAllEventsAtDeadline(
                    date.ToUniversalTime()));
        }

        foreach (var reminderEvent in reminderEvents)
        {
            var participants = reminderEvent.Participants
                .Select(p => p.UserId)
                .ToList();

            if (participants.Count > 0)
            {
                await mailerService.SendReminderMail(participants, reminderEvent);
                logger.LogInformation("Reminder email prepared for event: {Title}, participants: {Count}", reminderEvent.Title, participants.Count);
            }
        }
    }

    private static List<DateTimeOffset> GetDaysToCheckForDeadline(DateTimeOffset now)
    {
        if (now.DayOfWeek == DayOfWeek.Saturday ||
            now.DayOfWeek == DayOfWeek.Sunday)
        {
            return [];
        }

        var tomorrow = now.Date.AddDays(1);
        var daysToCheck = new List<DateTimeOffset> { tomorrow };

        if (now.DayOfWeek == DayOfWeek.Friday)
        {
            var sunday = now.AddDays(2);
            var monday = now.AddDays(3);
            daysToCheck.AddRange([sunday, monday]);
        }

        return daysToCheck;
    }
}


// OBS! PREVIOUS IMPLEMENTATION

//using EventFoodOrders.Entities;
//using EventFoodOrders.Repositories.Interfaces;
//using EventFoodOrders.Utilities;

//namespace EventFoodOrders.Services;

//// TODO: Replace Timer with Task.Delay + CancellationToken for cleaner async handling and proper shutdown support
//// Read backend section at https://dataductus.atlassian.net/wiki/spaces/EFO/pages/3468951557/ToDo+s+som+r+kvar.
//public class ReminderService(ILogger<ReminderService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
//{
//    private Timer? _timer;

//    protected override Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        var now = DateTime.Now;
//        var nextRunTime = DateTime.Today.AddHours(7).AddMinutes(30); //Run service at 7:30 every morning.
//        if (now > nextRunTime)
//        {
//            nextRunTime = nextRunTime.AddDays(1);
//        }

//        var initialDelay = nextRunTime - now;

//        logger.LogInformation($"Reminder service will start in {initialDelay.TotalSeconds} seconds.");

//        _timer = new Timer(async void (state) =>
//        {
//            try
//            {
//                await DoWork(state);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, ex.Message);
//            }
//        }, null, initialDelay, TimeSpan.FromDays(1));

//        return Task.CompletedTask;
//    }

//    private async Task DoWork(object? state)
//    {
//        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
//        {
//            return;
//        }
//        List<DateTime> daysToCheck = [DateTime.Now.AddDays(1)]; //ToDo: Hardcoded 1 workday before deadline reminders, could be dynamic
//        if (daysToCheck[0].DayOfWeek == DayOfWeek.Saturday)
//        {
//            daysToCheck.Add(DateTime.Now.AddDays(2));
//            daysToCheck.Add(DateTime.Now.AddDays(3));
//        }

//        logger.LogInformation("Reminder service started at: {time}", DateTimeOffset.Now);
//        using (var scope = scopeFactory.CreateScope())
//        {
//            var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
//            var mailerService = scope.ServiceProvider.GetRequiredService<IMailerService>();

//            List<Event> reminderEvents = [];
//            foreach (var dateTime in daysToCheck)
//            {
//                reminderEvents.AddRange(await uow.EventRepository.GetAllEventsAtDeadline(dateTime));
//            }

//            if (reminderEvents.Count > 0)
//            {
//                foreach (var item in reminderEvents)
//                {
//                    var participants = item.Participants
//                        .Where(p => p.ResponseType == ReType.Pending)
//                        .Select(p  => p.UserId)
//                        .ToList();
//                    participants.Add(item.OwnerId);
//                    if (participants.Count <= 0) continue;

//                    // TODO! Uncomment this method
//                    // The mocked service will write to file during development instead of sending mails, this method can therefor be called in development.
//                    // await mailerService.SendReminderMail(participants, item);

//                    logger.LogInformation("Reminder service running for participant list for event: " + item.Title);
//                }
//            }
//        }
//    }

//    public override Task StopAsync(CancellationToken cancellationToken)
//    {
//        logger.LogInformation("Reminder service stopped.");
//        _timer?.Change(Timeout.Infinite, 0);
//        return base.StopAsync(cancellationToken);
//    }
//}
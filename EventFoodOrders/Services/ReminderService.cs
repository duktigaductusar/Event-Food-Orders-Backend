using EventFoodOrders.Repositories.Interfaces;
using EventFoodOrders.Services.Interfaces;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Services;

public class ReminderService(ILogger<ReminderService> logger, IUoW _uow, IUserService _userService) : BackgroundService
{
    private Timer _timer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.Now;
        var nextRunTime = DateTime.Today.AddHours(7).AddMinutes(30);
        if (now > nextRunTime)
        {
            nextRunTime = nextRunTime.AddDays(1);
        }

        var initialDelay = nextRunTime - now;
        
        logger.LogInformation($"Reminder service will start in {initialDelay.TotalSeconds} seconds.");

        // _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromDays(1));
        _timer = new Timer(async void (state) =>
        {
            try
            {
                await DoWork(state);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error running BackgroundReminder: ", ex.Message);
            }
        }, null, initialDelay, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async Task DoWork(object state)
    {
        var now = DateTime.Now;
        logger.LogInformation("Reminder service started at: {time}", DateTimeOffset.Now);
        // Our logic here
        var events = await _uow.EventRepository.GetAllEventsAtDeadline(now);
        if (events.Count > 0)
        {
            foreach (var item in events)
            {
                var participants = item.Participants
                    .Where(p => p.ResponseType == ReType.Pending)
                    .Select(p  => p.UserId)
                    .ToList();

                if (participants.Count <= 0) continue;
                await _userService.SendEmail(participants, "");
            }
        }
        //################
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Reminder service stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }
}
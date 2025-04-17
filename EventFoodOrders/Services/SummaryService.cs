using EventFoodOrders.Entities;
using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Services;

public class SummaryService(
    ILogger<SummaryService> logger,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration
) : BackgroundService
{
    private readonly TimeSpan PollingInterval = TimeSpan.FromMinutes(
        double.TryParse(configuration["PollingIntervalSummaryService"], out var minutes)
            ? minutes
            : throw new ArgumentException("PollingIntervalSummaryService must be a number")
    );

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SummaryService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
                var mailerService = scope.ServiceProvider.GetRequiredService<IMailerService>();
                var now = DateTime.Now;
                var dueEvents = await uow.EventRepository.GetEventsWithPassedDeadlines(4);

                if (!dueEvents.Any())
                {
                    logger.LogInformation("No due events found. Checking again in {Interval} minutes.", PollingInterval.TotalMinutes);
                }
                else
                {
                    await HandleSendSummaryMail(dueEvents, uow, mailerService);
                }

                await Task.Delay(PollingInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("SummaryService is stopping due to cancellation.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in SummaryService. Retrying in 5 minutes.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("SummaryService stopped.");
    }

    private async Task HandleSendSummaryMail(
        IEnumerable<Event> dueEvents,
        IUoW uow,
        IMailerService mailerService
    )
    {
        foreach (var dueEvent in dueEvents)
        {
            try
            {
                await mailerService.SendSummaryMail(dueEvent);
                await uow.EventRepository.UpdateEventToStatusDeadlinePassed(dueEvent);
                logger.LogInformation("Summary mail sent for event {EventId}.", dueEvent.Id);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to send summary mail for event {EventId}.", dueEvent.Id);
            }
        }
    }
}


// OBS! PREVIOUS IMPLEMENTATION
//using EventFoodOrders.Entities;
//using EventFoodOrders.Repositories.Interfaces;

//namespace EventFoodOrders.Services;

//// TODO: Replace Timer with Task.Delay + CancellationToken for cleaner async handling and proper shutdown support.
//// Read backend section at https://dataductus.atlassian.net/wiki/spaces/EFO/pages/3468951557/ToDo+s+som+r+kvar.
//public class SummaryService(ILogger<SummaryService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
//{
//    private Timer? _timer;
//    private readonly DateTime _now = DateTime.Now;
//    private Event? _nextUpcomingEvent;


//    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
//    {
//        await ScheduleNextRun();
//    }

//    public void TimerCallback(object? state)
//    {
//        _ = Task.Run(async () =>
//        {
//            try
//            {
//                await DoWork(state);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, ex.Message);
//            }
//            finally
//            {
//                await ScheduleNextRun();
//            }
//        });
//    }


//    private async Task ScheduleNextRun()
//    {
//        using var scope = scopeFactory.CreateScope();
//        var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
//        _nextUpcomingEvent = await uow.EventRepository.GetNextUpcomingDeadline();
//        if (_nextUpcomingEvent == null)
//        {
//            if (_timer != null)
//            {
//                logger.LogInformation("Next upcoming deadline was found null, checking again in 1 hour");
//                _timer.Change(TimeSpan.FromHours(1), Timeout.InfiniteTimeSpan);
//            }
//        }
//        else
//        {
//            var initialDelay = _nextUpcomingEvent.Deadline - _now;
//            logger.LogInformation($"Summary service will start in {initialDelay.TotalSeconds} seconds.");
//            if (_timer == null)
//            {
//                _timer = new Timer(TimerCallback, null, initialDelay, Timeout.InfiniteTimeSpan);
//            }
//            else
//            {
//                _timer.Change(initialDelay, Timeout.InfiniteTimeSpan);
//            }
//        }
//    }

//    private async Task DoWork(object? state)
//    {
//        logger.LogInformation($"Summary service started at: {_now.Hour}:{_now.Minute}:{_now.Second}.");

//        using var scope = scopeFactory.CreateScope();
//        var mailerService = scope.ServiceProvider.GetService<IMailerService>();

//        if (mailerService != null && _nextUpcomingEvent != null)
//        {
//            await mailerService.SendSummaryMail(_nextUpcomingEvent);
//        }

//        _nextUpcomingEvent = null;
//    }

//}

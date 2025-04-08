using EventFoodOrders.Entities;
using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Services;

public class SummaryService(ILogger<SummaryService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private Timer _timer;
    private readonly DateTime _now = DateTime.Now;
    private Event _nextUpcomingEvent;


    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ScheduleNextRun();
        return Task.CompletedTask;
    }

    public async void TimerCallback(object state)
    {
        try
        {
            await DoWork(state);
            ScheduleNextRun();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            ScheduleNextRun();
        }
    }

    private void ScheduleNextRun()
    {
        using var scope = scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
        _nextUpcomingEvent = uow.EventRepository.GetNextUpcomingDeadline();
        if (_nextUpcomingEvent == null)
        {
            logger.LogInformation("Next upcoming deadline was found null, checking again in 1 hour");
            _timer.Change(TimeSpan.FromHours(1), Timeout.InfiniteTimeSpan);
        }
        else
        { 
            var initialDelay = _nextUpcomingEvent.Deadline - _now;
            logger.LogInformation($"Summary service will start in {initialDelay.TotalSeconds} seconds.");
            if (_timer == null)
            {
                _timer = new Timer(TimerCallback, null, initialDelay, Timeout.InfiniteTimeSpan);
            }
            else
            {
                _timer.Change(initialDelay, Timeout.InfiniteTimeSpan);
            }
        }
    }

    private async Task DoWork(object state)
    {
        logger.LogInformation($"Summary service started at: {_now.Hour}:{_now.Minute}:{_now.Second}.");
        
        using var scope = scopeFactory.CreateScope();
        var mailerService = scope.ServiceProvider.GetService<IMailerService>();
        await mailerService.SendSummaryMail(_nextUpcomingEvent);
        _nextUpcomingEvent = null;
    }

}
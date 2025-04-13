using EventFoodOrders.Entities;
using EventFoodOrders.Repositories.Interfaces;

namespace EventFoodOrders.Services;

public class SummaryService(ILogger<SummaryService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private Timer? _timer;
    private readonly DateTime _now = DateTime.Now;
    private Event? _nextUpcomingEvent;


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await ScheduleNextRun();
    }

    public void TimerCallback(object? state)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await DoWork(state);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            finally
            {
                await ScheduleNextRun();
            }
        });
    }


    private async Task ScheduleNextRun()
    {
        using var scope = scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUoW>();
        _nextUpcomingEvent = await uow.EventRepository.GetNextUpcomingDeadline();
        if (_nextUpcomingEvent == null)
        {
            if (_timer != null)
            {
                logger.LogInformation("Next upcoming deadline was found null, checking again in 1 hour");
                _timer.Change(TimeSpan.FromHours(1), Timeout.InfiniteTimeSpan);
            }
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

    private async Task DoWork(object? state)
    {
        logger.LogInformation($"Summary service started at: {_now.Hour}:{_now.Minute}:{_now.Second}.");
        
        using var scope = scopeFactory.CreateScope();
        var mailerService = scope.ServiceProvider.GetService<IMailerService>();
        
        if (mailerService != null && _nextUpcomingEvent != null)
        {
            await mailerService.SendSummaryMail(_nextUpcomingEvent);
        }
        
        _nextUpcomingEvent = null;
    }

}
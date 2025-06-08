# Mail

Sending an email when an event's deadline has passed is a common pattern in backend systems. You essentially want a **background job** that:

1. Periodically checks your database for expired deadlines.
2. Sends email notifications to event creators.

Here’s a breakdown of **recommended approaches** in ASP.NET Core with references, pros, and cons.

---

## ✅ Recommended Approaches

### 1. **Use a Hosted Service (`IHostedService`)**
ASP.NET Core lets you run background tasks using [hosted services](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services).

You can write a class like:

```csharp
public class EventDeadlineNotifier : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventDeadlineNotifier> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // Or use config

    public EventDeadlineNotifier(IServiceProvider serviceProvider, ILogger<EventDeadlineNotifier> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EventDbContext>();
            var mailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var expiredEvents = await db.Events
                .Where(e => e.Deadline < DateTime.UtcNow && !e.Notified)
                .ToListAsync(stoppingToken);

            foreach (var ev in expiredEvents)
            {
                await mailService.SendToCreatorAsync(ev);
                ev.Notified = true;
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
```

Register it in `Program.cs`:
```csharp
builder.Services.AddHostedService<EventDeadlineNotifier>();
```

✅ **Pros**:
- Fully managed inside your app.
- No external dependencies.
- Works well for small-to-medium load.

❌ **Cons**:
- Runs only while the app is up.
- No retry mechanism if the app crashes during send.

📖 Docs: https://learn.microsoft.com/en-us/dotnet/core/extensions/background-services

---

### 2. **Use Hangfire for Scheduled Jobs**
[Hangfire](https://www.hangfire.io/) is a production-ready library for background jobs in .NET.

You can define:

```csharp
RecurringJob.AddOrUpdate<IEventReminderService>(
    "notify-expired-events",
    service => service.NotifyExpiredEventsAsync(),
    Cron.MinuteInterval(5)); // Every 5 minutes
```

Service:
```csharp
public class EventReminderService : IEventReminderService
{
    public async Task NotifyExpiredEventsAsync()
    {
        // check DB and send emails
    }
}
```

✅ **Pros**:
- UI Dashboard
- Persistent storage of job history
- Retry, error handling, cron expressions

❌ **Cons**:
- Adds external dependency
- Needs storage backend (e.g., SQL Server)

📖 Docs: https://docs.hangfire.io/en/latest/

---

### 3. **Use Quartz.NET for Advanced Scheduling**
[Quartz.NET](https://www.quartz-scheduler.net/) is a powerful job scheduler.

Define a job class:

```csharp
public class NotifyExpiredEventsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // check DB, send mail, etc.
    }
}
```

Register in `Program.cs`:

```csharp
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("NotifyJob");
    q.AddJob<NotifyExpiredEventsJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever()));
});
builder.Services.AddQuartzHostedService();
```

✅ **Pros**:
- Rich scheduling (cron, calendars, triggers)
- Well-suited for complex job workflows

❌ **Cons**:
- Heavier than Hangfire
- More setup/config required

📖 Docs: https://www.quartz-scheduler.net/documentation/

---

### 4. **Use Azure Functions or AWS Lambda (if cloud-hosted)**
If you're on Azure or AWS:

- Use a **timer-triggered Azure Function** or **CloudWatch Event Rule** + Lambda to:
  - Query expired events
  - Send emails

✅ **Pros**:
- Scalable, serverless, resilient
- Independent of your main app's uptime

❌ **Cons**:
- Requires cloud infra
- More moving parts and deployment steps

📖 Azure Timer Trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer

---

## 📌 Additional Best Practices

- ✅ Use **flags** in your DB like `Notified` to avoid double-sending.
- ✅ Store logs and email outcomes for auditing.
- ✅ Use **dependency injection** properly in background services (`IEmailService`, `DbContext`, etc.).
- ✅ Externalize the check interval to configuration (`appsettings.json`).

---

## 🔚 Summary Table

| Approach               | Suitable For               | Persistence | Retry | Complexity | Recommended |
|------------------------|----------------------------|-------------|-------|------------|-------------|
| `IHostedService`       | Internal, simple tasks     | ❌           | ❌     | Low        | ✅ Yes       |
| Hangfire              | Production-grade jobs      | ✅           | ✅     | Medium     | ✅ Yes       |
| Quartz.NET             | Complex scheduling         | ✅           | ❌     | High       | Optional     |
| Azure Functions/Lambda | Cloud-first, scalable      | ✅           | ✅     | Medium     | ✅ If cloud  |

---


## Graph API sending emails on your app's behalf

Graph API endpoint:

```
POST https://graph.microsoft.com/v1.0/users/{senderEmail}/sendMail
```

This is the correct endpoint for sending email *as a specific user* or *from a shared mailbox*.

### Example request body (you're already using this):
```json
{
  "message": {
    "subject": "Reminder: Event Deadline Passed",
    "body": {
      "contentType": "Text",
      "content": "The deadline for your event has passed."
    },
    "toRecipients": [
      {
        "emailAddress": {
          "address": "creator@example.com"
        }
      }
    ]
  },
  "saveToSentItems": "false"
}
```

---

## 📌 Requirements for sending email using Graph

### ✅ 1. **App registration** in Microsoft Entra
Make sure your app is registered in Entra (Azure AD) and you're using the **Graph API client credentials flow** or **delegated flow** correctly.

### ✅ 2. **Proper permissions**
You need **one of these permissions** granted and **admin consented**:

#### If using **delegated permissions** (on behalf of a signed-in user):
- `Mail.Send`

#### If using **application permissions** (no user login, backend app like yours):
- `Mail.Send`
- `Mail.Send.Shared` (if sending from shared mailboxes)

🔐 Confirm in Azure Portal → **App registrations** → **API permissions**

📖 Docs: [https://learn.microsoft.com/en-us/graph/permissions-reference#mail-permissions](https://learn.microsoft.com/en-us/graph/permissions-reference#mail-permissions)

---

## 🧱 How to design it in your app

Now that you can send emails using Graph, here’s how to structure the **background job** to send deadline notifications.

### Step 1: Add a `BackgroundService`
```csharp
public class EventDeadlineNotifier : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<EventDeadlineNotifier> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

    public EventDeadlineNotifier(IServiceProvider provider, ILogger<EventDeadlineNotifier> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EventDbContext>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var now = DateTime.UtcNow;
            var expiredEvents = await db.Events
                .Where(e => e.Deadline < now && !e.NotificationSent)
                .ToListAsync(stoppingToken);

            foreach (var ev in expiredEvents)
            {
                var creatorEmail = ev.CreatedByEmail;
                await userService.SendEmail([ev.CreatedById], "Your event deadline has passed.");
                ev.NotificationSent = true;
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
```

### Step 2: Register it in `Program.cs`
```csharp
builder.Services.AddHostedService<EventDeadlineNotifier>();
```

---

## ✅ Summary

| Feature                             | Status |
|-------------------------------------|--------|
| Microsoft Entra + Graph for email   | ✅ Yes |
| Your app can send email             | ✅ Already implemented in `UserService` |
| Can run on a schedule               | ✅ Use `BackgroundService` or Hangfire |
| Permissions required                | ✅ `Mail.Send` (delegated or app-level) |
| Production-safe                     | ✅ Yes, as long as rate-limits & error-handling are in place |

---

## 📚 References

- [Send mail using Microsoft Graph](https://learn.microsoft.com/en-us/graph/api/user-sendmail?view=graph-rest-1.0&tabs=http)
- [Use app-only authentication with Microsoft Graph](https://learn.microsoft.com/en-us/graph/auth-v2-service)
- [ASP.NET Core background tasks](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services)

---

More functionality:

- Add retry logic and error handling
- Use a shared mailbox (with `Mail.Send.Shared`)
- Implement with Hangfire for better observability
- Log email sending results to a table for tracking

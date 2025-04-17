namespace EventFoodOrders.Utilities;

public static class ReType
{
    public const string Pending = "PENDING";
    public const string AttendingOnline = "ATTENDING_ONLINE";
    public const string AttendingOffice = "ATTENDING_OFFICE";
    public const string NotAttending = "NOT_ATTENDING";
}

public static class EventStatus
{
    public const string BeforeDeadline = "BEFORE_DEADLINE";
    public const string DeadlinePassed = "DEADLINE_PASSED";
    public const string DatePassed = "DATE_PASSED";
}

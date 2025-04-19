namespace EventFoodOrders.Utilities;

public static class DateUtility
{
    public static DateTimeOffset GetSwedishDateTimeOffset()
    {
        string timeZoneId = OperatingSystem.IsWindows()
            ? "Central European Standard Time"
            : "Europe/Stockholm";
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
    }
}

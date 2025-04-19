namespace EventFoodOrders.Utilities;

public static class DateUtility
{
    public static string TimeZoneId {  get; set; } = OperatingSystem.IsWindows()
            ? "Central European Standard Time"
            : "Europe/Stockholm";

    public static DateTimeOffset GetSwedishDateTimeOffset(DateTime )
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
    }

    public static DateTimeOffset GetSwedishDateTimeOffsetNow()
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
    }
}

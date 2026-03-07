namespace DntSite.Web.Features.Common.Utils.DateTimeToolkit;

public static class DateTimeToolkitExtensions
{
    public static int GetMinute(this DateTime? dueDate) => dueDate?.ToIranTimeZoneDateTime().Minute ?? 0;

    public static int GetHour(this DateTime? dueDate) => dueDate?.ToIranTimeZoneDateTime().Hour ?? 0;

    public static DateTime? CombineDateWithTime([NotNullIfNotNull(nameof(baseDate))] this DateTime? baseDate,
        int hour,
        int minute)
        => baseDate?.ToIranTimeZoneDateTime().Date.Add(new TimeSpan(hour, minute, seconds: 0));

    public static DateTime CombineDateWithTime(this DateTime baseDate, int hour, int minute)
        => baseDate.ToIranTimeZoneDateTime().Date.Add(new TimeSpan(hour, minute, seconds: 0));
}

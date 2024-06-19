namespace VyazmaTech.Prachka.Domain.Kernel;

public static class DateTimeExtensions
{
    public static DateOnly AsDateOnly(this DateTime dateTime)
        => new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);

    public static TimeOnly AsTimeOnly(this DateTime dateTime)
        => new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Microsecond);
}
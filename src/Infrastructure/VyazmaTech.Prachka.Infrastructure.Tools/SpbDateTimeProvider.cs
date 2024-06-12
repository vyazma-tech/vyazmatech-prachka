using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.Tools;

public class SpbDateTimeProvider : IDateTimeProvider
{
    private static readonly TimeZoneInfo TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

    /// <summary>
    /// Gets new instance with current spb date time.
    /// </summary>
    public static SpbDateTime CurrentDateTime
        => new(DateTime.SpecifyKind(FromLocal(DateTime.Now).Value, DateTimeKind.Unspecified));

    /// <summary>
    /// Gets current spb date.
    /// </summary>
    public static DateOnly CurrentDate => DateOnly.FromDateTime(CurrentDateTime.Value);

    /// <summary>
    /// Gets current spb date time based on date time.
    /// </summary>
    /// <param name="dateTime">date time.</param>
    /// <returns>current spb date time.</returns>
    public static SpbDateTime FromLocal(DateTime dateTime)
    {
        return new SpbDateTime(TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo));
    }

    /// <summary>
    /// Gets spb date time based on utc date time.
    /// </summary>
    /// <param name="dateTime">utc date time.</param>
    /// <returns>spb date time.</returns>
    public static SpbDateTime FromUtc(DateTime dateTime)
    {
        return new SpbDateTime(TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo));
    }

    /// <summary>
    /// Get utc date time base on spb date time.
    /// </summary>
    /// <param name="dateTime">spb date time.</param>
    /// <returns>utc date time.</returns>
    public static DateTime ToUtc(SpbDateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime(dateTime.Value, TimeZoneInfo, TimeZoneInfo.Utc);
    }

    public DateOnly DateNow => DateOnly.FromDateTime(UtcNow);

    public DateTime UtcNow => DateTime.Now;

    public SpbDateTime SpbDateTimeNow => CurrentDateTime;

    public DateOnly SpbDateOnlyNow => CurrentDateTime.AsDateOnly();
}
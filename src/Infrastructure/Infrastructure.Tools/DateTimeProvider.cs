using Domain.Common.Abstractions;

namespace Infrastructure.Tools;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.Now;
}
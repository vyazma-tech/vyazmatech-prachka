using Domain.Common.Abstractions;

namespace Infrastructure.Tools;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
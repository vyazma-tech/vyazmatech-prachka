using Domain.Kernel;

namespace Infrastructure.Tools;

public class DateTimeProvider : IDateTimeProvider
{
    public DateOnly DateNow => DateOnly.FromDateTime(UtcNow.Date);
    public DateTime UtcNow => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
}
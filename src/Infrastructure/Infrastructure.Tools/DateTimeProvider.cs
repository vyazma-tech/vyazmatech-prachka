using Domain.Kernel;

namespace Infrastructure.Tools;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
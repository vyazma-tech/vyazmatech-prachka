namespace Domain.Kernel;

public interface IDateTimeProvider
{
    DateOnly DateNow { get; }
    DateTime UtcNow { get; }
}
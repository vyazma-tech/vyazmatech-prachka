namespace Domain.Kernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
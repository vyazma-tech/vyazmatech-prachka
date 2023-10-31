namespace Domain.Common.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
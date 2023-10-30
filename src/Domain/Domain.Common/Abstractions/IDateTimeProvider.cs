namespace Domain.Common.Abstractions;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
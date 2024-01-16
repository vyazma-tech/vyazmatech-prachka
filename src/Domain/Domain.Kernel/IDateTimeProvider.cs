using Domain.Common.Abstractions;

namespace Domain.Kernel;

public interface IDateTimeProvider
{
    DateOnly DateNow { get; }
    DateTime UtcNow { get; }
    SpbDateTime SpbDateTimeNow { get; }
    DateOnly SpbDateOnlyNow { get; }
}
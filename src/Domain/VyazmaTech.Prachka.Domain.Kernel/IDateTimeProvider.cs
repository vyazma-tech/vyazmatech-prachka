using VyazmaTech.Prachka.Domain.Common.Abstractions;

namespace VyazmaTech.Prachka.Domain.Kernel;

public interface IDateTimeProvider
{
    DateOnly DateNow { get; }
    DateTime UtcNow { get; }
    SpbDateTime SpbDateTimeNow { get; }
    DateOnly SpbDateOnlyNow { get; }
}
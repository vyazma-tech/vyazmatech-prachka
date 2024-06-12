using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.Tools;

public sealed class DefaultTimeProvider : IDateTimeProvider
{
    public DateOnly DateNow => UtcNow.AsDateOnly();
    public DateTime UtcNow => DateTime.UtcNow;
}
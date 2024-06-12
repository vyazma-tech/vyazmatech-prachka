using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes queue activity boundaries.
/// i.e: 1pm - 5pm.
/// </summary>
public sealed class QueueActivityBoundaries : ValueObject
{
    private QueueActivityBoundaries(TimeOnly activeFrom, TimeOnly activeUntil)
    {
        ActiveFrom = activeFrom;
        ActiveUntil = activeUntil;
    }

    /// <summary>
    /// Gets queue active from time.
    /// </summary>
    public TimeOnly ActiveFrom { get; }

    /// <summary>
    /// Gets queue active until time.
    /// </summary>
    public TimeOnly ActiveUntil { get; }

    /// <summary>
    /// Validates and creates queue activity boundaries instance.
    /// </summary>
    /// <param name="activeFrom">queue active from.</param>
    /// <param name="activeUntil">queue active until.</param>
    /// <returns>constructed <see cref="QueueActivityBoundaries" /> instance.</returns>
    /// <remarks>returns failure result, when provided time range is empty.</remarks>
    public static QueueActivityBoundaries Create(TimeOnly activeFrom, TimeOnly activeUntil)
    {
        if (activeFrom >= activeUntil)
            throw new UserInvalidInputException(DomainErrors.QueueActivityBoundaries.EmptyRange);

        return new QueueActivityBoundaries(activeFrom, activeUntil);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ActiveFrom;
        yield return ActiveUntil;
    }
}
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

/// <summary>
/// Describes queue capacity model.
/// </summary>
public sealed class Capacity : ValueObject
{
#pragma warning disable CS8618
    private Capacity() { }
#pragma warning restore CS8618
    private Capacity(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets capacity.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Validates capacity and creates capacity instance.
    /// </summary>
    /// <param name="capacity">capacity.</param>
    /// <returns>constructed capacity instance.</returns>
    /// <remarks>returns failure result, when capacity is negative.</remarks>
    public static Result<Capacity> Create(int capacity)
    {
        if (capacity < 0)
        {
            var exception = new DomainException(DomainErrors.Capacity.Negative);
            return new Result<Capacity>(exception);
        }

        return new Capacity(capacity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
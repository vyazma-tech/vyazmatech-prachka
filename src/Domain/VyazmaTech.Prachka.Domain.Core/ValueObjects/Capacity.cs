using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes queue capacity model.
/// </summary>
public sealed class Capacity : ValueObject
{
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
    public static Capacity Create(int capacity)
    {
        if (capacity < 0)
            throw new UserInvalidInputException(DomainErrors.Capacity.Negative);

        return new Capacity(capacity);
    }

    public static bool operator <=(Capacity? left, Capacity? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Value <= right.Value;
    }

    public static bool operator >=(Capacity? left, Capacity? right)
    {
        bool equals = left == right;

        if (equals is false)
            return !(left <= right);

        return equals;
    }

    public static implicit operator int(Capacity capacity) => capacity.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

public sealed class Capacity : ValueObject
{
    private Capacity(int value)
        => Value = value;

    public int Value { get; }

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
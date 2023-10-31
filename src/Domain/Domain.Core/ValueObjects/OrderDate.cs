using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

public sealed class OrderDate : ValueObject
{
    private OrderDate(DateTime value)
        => Value = value;

    public DateTime Value { get; }

    public static Result<OrderDate> Create(DateTime dateTime, IDateTimeProvider dateTimeProvider)
    {
        if (dateTime <= dateTimeProvider.UtcNow)
        {
            var exception = new DomainException(DomainErrors.OrderDate.InThePast);
            return new Result<OrderDate>(exception);
        }

        return new OrderDate(dateTime);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
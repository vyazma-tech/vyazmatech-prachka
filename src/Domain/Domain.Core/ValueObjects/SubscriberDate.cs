using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

public sealed class SubscriberDate : ValueObject
{
    private SubscriberDate(DateTime value)
        => Value = value;

    public DateTime Value { get; }

    public static Result<SubscriberDate> Create(DateTime dateTime, IDateTimeProvider dateTimeProvider)
    {
        if (dateTime <= dateTimeProvider.UtcNow)
        {
            var exception = new DomainException(DomainErrors.SubscriberDate.InThePast);
            return new Result<SubscriberDate>(exception);
        }

        return new SubscriberDate(dateTime);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
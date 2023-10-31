using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

public sealed class QueueDate : ValueObject
{
    public const int Week = 7;

    private QueueDate(DateTime value)
        => Value = value;

    public DateTime Value { get; }

    public static Result<QueueDate> Create(DateTime value, IDateTimeProvider dateTimeProvider)
    {
        if (value <= dateTimeProvider.UtcNow)
        {
            var exception = new DomainException(DomainErrors.QueueDate.InThePast);
            return new Result<QueueDate>(exception);
        }

        if (value >= dateTimeProvider.UtcNow.AddDays(Week))
        {
            var exception = new DomainException(DomainErrors.QueueDate.NotNextWeek);
            return new Result<QueueDate>(exception);
        }

        return new QueueDate(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
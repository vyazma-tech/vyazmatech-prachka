using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

public sealed class QueueActivityBoundaries : ValueObject
{
    private QueueActivityBoundaries(TimeOnly activeFrom, TimeOnly activeUntil)
    {
        ActiveFrom = activeFrom;
        ActiveUntil = activeUntil;
    }

    public TimeOnly ActiveFrom { get; }
    public TimeOnly ActiveUntil { get; }

    public static Result<QueueActivityBoundaries> Create(TimeOnly activeFromUtc, TimeOnly activeUntil)
    {
        if (activeFromUtc >= activeUntil)
        {
            var exception = new DomainException(DomainErrors.QueueActivityBoundaries.EmptyRange);
            return new Result<QueueActivityBoundaries>(exception);
        }

        return new QueueActivityBoundaries(activeFromUtc, activeUntil);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ActiveFrom;
        yield return ActiveUntil;
    }
}
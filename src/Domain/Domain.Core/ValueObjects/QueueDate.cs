using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Kernel;

namespace Domain.Core.ValueObjects;

/// <summary>
/// Describes queue date model.
/// </summary>
public sealed class QueueDate : ValueObject
{
#pragma warning disable CS8618
    private QueueDate() { }
#pragma warning restore CS8618
    public const int Week = 7;

    private QueueDate(DateOnly value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets queue date.
    /// </summary>
    public DateOnly Value { get; }

    /// <summary>
    /// Validates and creates queue date instance.
    /// </summary>
    /// <param name="assignmentDate">date, which queue is assigned to.</param>
    /// <param name="dateTimeProvider">time provider.</param>
    /// <returns>constructed queue date instance.</returns>
    /// <remarks>returns failure result, when assignment date is not around closest 7 days.</remarks>
    public static Result<QueueDate> Create(DateOnly assignmentDate, IDateTimeProvider dateTimeProvider)
    {
        if (assignmentDate < dateTimeProvider.DateNow)
        {
            var exception = new DomainException(DomainErrors.QueueDate.InThePast);
            return new Result<QueueDate>(exception);
        }

        if (assignmentDate > dateTimeProvider.DateNow.AddDays(Week))
        {
            var exception = new DomainException(DomainErrors.QueueDate.NotNextWeek);
            return new Result<QueueDate>(exception);
        }

        return new QueueDate(assignmentDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes queue date model.
/// </summary>
public sealed class QueueDate : ValueObject
{
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
            return new Result<QueueDate>(DomainErrors.QueueDate.InThePast);
        }

        if (assignmentDate > dateTimeProvider.DateNow.AddDays(Week))
        {
            return new Result<QueueDate>(DomainErrors.QueueDate.NotNextWeek);
        }

        return new QueueDate(assignmentDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
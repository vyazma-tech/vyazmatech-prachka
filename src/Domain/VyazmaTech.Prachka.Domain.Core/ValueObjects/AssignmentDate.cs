using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes queue date model.
/// </summary>
public sealed class AssignmentDate : ValueObject
{
    public const int Week = 7;

    private AssignmentDate(DateOnly value)
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
    /// <param name="currentDate">current date.</param>
    /// <returns>constructed queue date instance.</returns>
    /// <remarks>returns failure result, when assignment date is not around closest 7 days.</remarks>
    public static AssignmentDate Create(DateOnly assignmentDate, DateOnly currentDate)
    {
        if (assignmentDate < currentDate)
            throw new DomainInvalidOperationException(DomainErrors.QueueDate.InThePast);

        if (assignmentDate > currentDate.AddDays(Week))
            throw new DomainInvalidOperationException(DomainErrors.QueueDate.NotNextWeek);

        return new AssignmentDate(assignmentDate);
    }

    public static implicit operator DateOnly(AssignmentDate assignmentDate) => assignmentDate.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
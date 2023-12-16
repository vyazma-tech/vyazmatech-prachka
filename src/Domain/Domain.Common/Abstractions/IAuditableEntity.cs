namespace Domain.Common.Abstractions;

public interface IAuditableEntity
{
    DateOnly CreationDate { get; }

    DateTime? ModifiedOn { get; }
}
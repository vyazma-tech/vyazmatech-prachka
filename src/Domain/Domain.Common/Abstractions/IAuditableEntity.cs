namespace Domain.Common.Abstractions;

public interface IAuditableEntity
{
    DateTime CreationDate { get; }

    DateTime? ModifiedOn { get; }
}
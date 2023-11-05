namespace Domain.Common.Abstractions;

public interface IAuditableEntity
{
    DateTime QueueDate { get; }

    DateTime? ModifiedOn { get; }
}
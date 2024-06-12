namespace VyazmaTech.Prachka.Domain.Common.Abstractions;

public interface IAuditableEntity
{
    DateOnly CreationDate { get; }

    DateTime? ModifiedOnUtc { get; }
}
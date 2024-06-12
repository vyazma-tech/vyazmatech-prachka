using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Subscriptions;

public abstract class SubscriptionEntity : Entity, IAuditableEntity
{
    protected SubscriptionEntity(
        Guid id,
        Guid user,
        DateOnly creationDate,
        DateTime? modifiedOn = null)
        : base(id)
    {
        User = user;
        CreationDate = creationDate;
        ModifiedOnUtc = modifiedOn;
    }

    public Guid User { get; private set; }

    public DateOnly CreationDate { get; }

    public DateTime? ModifiedOnUtc { get; }
}
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Subscription;

public abstract class SubscriptionEntity : Entity, IAuditableEntity
{
    protected SubscriptionEntity(
        Guid id,
        Guid user,
        DateOnly creationDateUtc,
        DateTime? modifiedOn = null)
        : base(id)
    {
        User = user;
        CreationDate = creationDateUtc;
        ModifiedOnUtc = modifiedOn;
    }

    public Guid User { get; private set; }

    public DateOnly CreationDate { get; }

    public DateTime? ModifiedOnUtc { get; }
}
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Subscription;

public abstract class SubscriptionEntity : Entity, IAuditableEntity
{
    protected SubscriptionEntity(
        Guid id,
        Guid user,
        DateOnly creationDateUtc,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        User = user;
        CreationDate = creationDateUtc;
        ModifiedOn = modifiedOn;
    }

    public Guid User { get; private set; }

    public DateOnly CreationDate { get; }

    public SpbDateTime? ModifiedOn { get; }
}
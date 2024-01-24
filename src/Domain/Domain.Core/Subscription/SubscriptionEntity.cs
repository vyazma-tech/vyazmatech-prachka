using Domain.Common.Abstractions;
using Domain.Kernel;

namespace Domain.Core.Subscription;

public abstract class SubscriptionEntity : Entity, IAuditableEntity
{
    public SubscriptionEntity(
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
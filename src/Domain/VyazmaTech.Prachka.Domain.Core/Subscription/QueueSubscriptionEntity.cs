using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.Subscription;

public sealed class QueueSubscriptionEntity : SubscriptionEntity
{
    private readonly HashSet<Guid> _queueIds;

    public QueueSubscriptionEntity(
        Guid id,
        Guid user,
        DateOnly creationDateUtc,
        HashSet<Guid> queueIds,
        DateTime? modifiedOn = null)
        : base(id, user, creationDateUtc, modifiedOn)
    {
        _queueIds = queueIds;
    }

    public IReadOnlyCollection<Guid> SubscribedQueues => _queueIds;

    public void Subscribe(Guid id)
    {
        if (_queueIds.Contains(id))
            throw new DomainInvalidOperationException(DomainErrors.Subscription.ContainsQueueWithId(id));

        _queueIds.Add(id);
    }

    public void Unsubscribe(Guid id)
    {
        if (_queueIds.Contains(id) is false)
            throw new DomainInvalidOperationException(DomainErrors.Subscription.QueueIsNotInSubscription(id));

        _queueIds.Remove(id);
    }
}
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.Subscriptions;

public sealed class QueueSubscriptionEntity : SubscriptionEntity
{
    private readonly HashSet<Guid> _queueIds;

    public QueueSubscriptionEntity(
        Guid id,
        Guid user,
        DateOnly creationDate,
        HashSet<Guid> queueIds,
        DateTime? modifiedOn = null)
        : base(id, user, creationDate, modifiedOn)
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
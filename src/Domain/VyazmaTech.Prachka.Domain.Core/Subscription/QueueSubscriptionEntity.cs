using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Queue;

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

    public Result<QueueEntity> Subscribe(QueueEntity queue)
    {
        if (_queueIds.Contains(queue.Id))
        {
            return new Result<QueueEntity>(DomainErrors.Subscription.ContainsQueueWithId(queue.Id));
        }

        _queueIds.Add(queue.Id);

        return queue;
    }

    public Result<QueueEntity> Unsubscribe(QueueEntity queue)
    {
        if (_queueIds.Contains(queue.Id) is false)
        {
            return new Result<QueueEntity>(DomainErrors.Subscription.QueueIsNotInSubscription(queue.Id));
        }

        _queueIds.Remove(queue.Id);

        return queue;
    }
}
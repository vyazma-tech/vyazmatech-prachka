using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.User;

namespace Domain.Core.Subscription;

public class QueueSubscriptionEntity : SubscriptionEntity
{
    private readonly List<QueueEntity> _subscribedQueues;

    public QueueSubscriptionEntity(UserEntity user, DateOnly creationDateUtc)
        : base(user, creationDateUtc)
    {
        _subscribedQueues = new List<QueueEntity>();
    }

#pragma warning disable CS8618
    protected QueueSubscriptionEntity() { }
#pragma warning restore CS8618

    public virtual IReadOnlyCollection<QueueEntity> SubscribedQueues => _subscribedQueues;

    /// <summary>
    /// Subscribes queue to the newsletter.
    /// </summary>
    /// <param name="queue">queue to be subscribed.</param>
    /// <returns>subscribed queue entity.</returns>
    /// <remarks>returns failure result, when queue is already subscribed.</remarks>
    public Result<QueueEntity> Subscribe(QueueEntity queue)
    {
        if (_subscribedQueues.Contains(queue))
        {
            var exception = new DomainException(DomainErrors.Subscription.ContainsQueueWithId(queue.Id));
            return new Result<QueueEntity>(exception);
        }

        _subscribedQueues.Add(queue);

        return queue;
    }

    /// <summary>
    /// Unsubscribes queue from the newsletter.
    /// </summary>
    /// <param name="queue">queue to be unsubscribed.</param>
    /// <returns>unsubscribed queue.</returns>
    /// <remarks>returns failure result, when queue is not subscribed.</remarks>
    public Result<QueueEntity> Unsubscribe(QueueEntity queue)
    {
        if (_subscribedQueues.Contains(queue) is false)
        {
            var exception = new DomainException(DomainErrors.Subscription.QueueIsNotInSubscription(queue.Id));
            return new Result<QueueEntity>(exception);
        }

        _subscribedQueues.Remove(queue);

        return queue;
    }
}
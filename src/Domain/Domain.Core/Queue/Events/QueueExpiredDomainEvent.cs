using Domain.Common.Abstractions;

namespace Domain.Core.Queue.Events;

/// <summary>
/// Queue expired and all orders, which are not paid,
/// should be removed from it. Subscriptions should be reset.
/// </summary>
public sealed class QueueExpiredDomainEvent : IDomainEvent
{
    public QueueExpiredDomainEvent(QueueEntity queue)
    {
        Queue = queue;
    }

    public QueueEntity Queue { get; }
}
using Domain.Common.Abstractions;

namespace Domain.Core.Queue.Events;

public sealed class QueueExpiredDomainEvent : IDomainEvent
{
    public QueueExpiredDomainEvent(QueueEntity queue)
    {
        Queue = queue;
    }

    public QueueEntity Queue { get; }
}
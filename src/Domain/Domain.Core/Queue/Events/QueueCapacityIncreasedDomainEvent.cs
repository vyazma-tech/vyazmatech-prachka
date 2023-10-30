using Domain.Common.Abstractions;

namespace Domain.Core.Queue.Events;

public sealed class QueueCapacityIncreasedDomainEvent : IDomainEvent
{
    public QueueCapacityIncreasedDomainEvent(QueueEntity queue)
    {
        Queue = queue;
    }

    public QueueEntity Queue { get; }
}
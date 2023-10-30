using Domain.Common.Abstractions;

namespace Domain.Core.Queue.Events;

public sealed class PositionAvailableDomainEvent : IDomainEvent
{
    public PositionAvailableDomainEvent(QueueEntity queue)
    {
        Queue = queue;
    }

    public QueueEntity Queue { get; }
}
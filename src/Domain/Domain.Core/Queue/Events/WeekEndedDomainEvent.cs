using Domain.Common.Abstractions;

namespace Domain.Core.Queue.Events;

public sealed class WeekEndedDomainEvent : IDomainEvent
{
    public WeekEndedDomainEvent(QueueEntity queue)
    {
        Queue = queue;
    }

    public QueueEntity Queue { get; }
}
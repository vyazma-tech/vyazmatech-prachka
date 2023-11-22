using Domain.Kernel;

namespace Domain.Core.Queue.Events;

/// <summary>
/// There is available position(s) in a queue. Subscribers
/// should be notified.
/// </summary>
public sealed class PositionAvailableDomainEvent : IDomainEvent
{
    public PositionAvailableDomainEvent(QueueEntity queue)
    {
        Queue = queue;
    }

    public QueueEntity Queue { get; }
}
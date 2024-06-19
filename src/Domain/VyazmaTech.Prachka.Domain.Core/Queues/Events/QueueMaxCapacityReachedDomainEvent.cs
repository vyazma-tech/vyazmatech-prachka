using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

/// <summary>
/// Queue max capacity reached, idempotent handler should schedule a job
/// that will try to notify about available position in a queue
/// </summary>
public sealed class QueueMaxCapacityReachedDomainEvent : IDomainEvent
{
    public QueueMaxCapacityReachedDomainEvent(Queue queue)
    {
        Queue = queue;
    }

    public Queue Queue { get; }
}
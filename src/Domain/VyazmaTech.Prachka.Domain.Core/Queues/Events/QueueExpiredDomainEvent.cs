using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

/// <summary>
/// Queue expired and all orders, which are not paid,
/// should be removed from it. Subscriptions should be reset.
/// </summary>
public sealed class QueueExpiredDomainEvent : IDomainEvent
{
    public QueueExpiredDomainEvent(Queue queue)
    {
        Queue = queue;
    }

    public Queue Queue { get; }
}
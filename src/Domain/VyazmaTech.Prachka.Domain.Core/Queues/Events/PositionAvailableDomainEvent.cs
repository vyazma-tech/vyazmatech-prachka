using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

/// <summary>
/// There is available position(s) in a queue. Subscribers
/// should be notified.
/// </summary>
public sealed class PositionAvailableDomainEvent : IDomainEvent
{
    private PositionAvailableDomainEvent(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }

    public static PositionAvailableDomainEvent From(Queue queue)
    {
        return new PositionAvailableDomainEvent(queue.Id);
    }
}
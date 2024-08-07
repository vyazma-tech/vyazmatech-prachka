using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

/// <summary>
/// There is available position(s) in a queue. Subscribers
/// should be notified.
/// </summary>
public sealed class PositionAvailableDomainEvent : IDomainEvent
{
    public PositionAvailableDomainEvent(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
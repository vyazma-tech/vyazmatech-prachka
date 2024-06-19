using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Orders.Events;

/// <summary>
/// Order is ready. If it has subscribers, they should
/// be notified. Should be removed from a current queue.
/// </summary>
public sealed class OrderReadyDomainEvent : IDomainEvent
{
    public OrderReadyDomainEvent(Orders.Order order)
    {
        Order = order;
    }

    public Orders.Order Order { get; }
}
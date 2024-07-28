using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Orders.Events;

/// <summary>
/// Order is paid. Should be deleted from future queues.
/// </summary>
public sealed class OrderPaidDomainEvent : IDomainEvent
{
    public OrderPaidDomainEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
}
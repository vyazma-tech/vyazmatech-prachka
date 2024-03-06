using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Order.Events;

/// <summary>
/// Order is paid. Should be deleted from future queues.
/// </summary>
public sealed class OrderPaidDomainEvent : IDomainEvent
{
    public OrderPaidDomainEvent(OrderEntity order)
    {
        Order = order;
    }

    public OrderEntity Order { get; }
}
using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderProlongedDomainEvent : IDomainEvent
{
    public OrderProlongedDomainEvent(OrderEntity order)
    {
        Order = order;
    }

    public OrderEntity Order { get; }
}
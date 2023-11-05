using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderCreatedDomainEvent : IDomainEvent
{
    public OrderCreatedDomainEvent(OrderEntity order)
    {
        Order = order;
    }

    public OrderEntity Order { get; }
}
using Domain.Common.Abstractions;

namespace Domain.Core.Orders.Events;

public sealed class OrderCreatedDomainEvent : IDomainEvent
{
    public OrderCreatedDomainEvent(Orders.OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public Orders.OrderEntity OrderEntity { get; }
}
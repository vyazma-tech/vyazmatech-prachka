using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderCreatedDomainEvent : IDomainEvent
{
    public OrderCreatedDomainEvent(OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public OrderEntity OrderEntity { get; }
}
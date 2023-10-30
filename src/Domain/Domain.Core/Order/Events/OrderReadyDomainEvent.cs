using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderReadyDomainEvent : IDomainEvent
{
    public OrderReadyDomainEvent(OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public OrderEntity OrderEntity { get; }
}
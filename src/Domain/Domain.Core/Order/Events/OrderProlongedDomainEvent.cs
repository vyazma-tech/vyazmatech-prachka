using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderProlongedDomainEvent : IDomainEvent
{
    public OrderProlongedDomainEvent(OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public OrderEntity OrderEntity { get; }
}
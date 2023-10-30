using Domain.Common.Abstractions;

namespace Domain.Core.Orders.Events;

public sealed class OrderProlongedDomainEvent : IDomainEvent
{
    public OrderProlongedDomainEvent(Orders.OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public Orders.OrderEntity OrderEntity { get; }
}
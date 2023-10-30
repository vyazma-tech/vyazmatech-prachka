using Domain.Common.Abstractions;

namespace Domain.Core.Orders.Events;

public sealed class OrderReadyDomainEvent : IDomainEvent
{
    public OrderReadyDomainEvent(Orders.OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public Orders.OrderEntity OrderEntity { get; }
}
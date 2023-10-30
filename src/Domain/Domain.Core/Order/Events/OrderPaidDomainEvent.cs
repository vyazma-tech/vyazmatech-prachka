using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderPaidDomainEvent : IDomainEvent
{
    public OrderPaidDomainEvent(OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public OrderEntity OrderEntity { get; }
}
using Domain.Common.Abstractions;

namespace Domain.Core.Orders.Events;

public sealed class OrderPaidDomainEvent : IDomainEvent
{
    public OrderPaidDomainEvent(Orders.OrderEntity orderEntity)
    {
        OrderEntity = orderEntity;
    }

    public Orders.OrderEntity OrderEntity { get; }
}
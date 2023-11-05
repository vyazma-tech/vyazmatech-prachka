using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

public sealed class OrderReadyDomainEvent : IDomainEvent
{
    public OrderReadyDomainEvent(OrderEntity order)
    {
        Order = order;
    }

    public OrderEntity Order { get; }
}
using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

/// <summary>
/// Order is prolonged to a most nearest queue.
/// Should be deleted from a current queue and be moved to a nearest queue.
/// </summary>
public sealed class OrderProlongedDomainEvent : IDomainEvent
{
    public OrderProlongedDomainEvent(OrderEntity order)
    {
        Order = order;
    }

    public OrderEntity Order { get; }
}
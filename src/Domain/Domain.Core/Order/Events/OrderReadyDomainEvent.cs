﻿using Domain.Common.Abstractions;

namespace Domain.Core.Order.Events;

/// <summary>
/// Order is ready. If it has subscribers, they should
/// be notified. Should be removed from a current queue.
/// </summary>
public sealed class OrderReadyDomainEvent : IDomainEvent
{
    public OrderReadyDomainEvent(OrderEntity order)
    {
        Order = order;
    }

    public OrderEntity Order { get; }
}
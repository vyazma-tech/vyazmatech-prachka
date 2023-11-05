# Order

## Domain events

- OrderPaidDomainEvent &mdash; order is paid and should
  be deleted from all future queues. Basically describes, that
  order is accepted, hence is being processed by laundry.


- OrderProlongedDomainEvent &mdash; order cannot be processed
  today, so it should be removed from current queue and
  be moved to the nearest queue.


- OrderReadyDomainEvent &mdash; order is ready for a pick up.

## API
### Properly create order:
```csharp
var order = Order.Create(...);

public static Result<OrderEntity> Create(UserEntity user, QueueEntity queue, DateTime creationDateUtc)
{
    var order = new OrderEntity(user, queue, creationDateUtc); 
    Result<OrderEntity> entranceResult = queue.Add(order);

    if (entranceResult.IsFaulted)
    {
        return entranceResult;
    }

    order.Raise(new OrderCreatedDomainEvent(order));

    return order;
}
```

Prolong order:
```csharp
order.Prolong(DateTime.UtcNow);
// handle OrderProlongedDomainEvent by handler and perform logic
```

# Queue
## Domain events

- PositionAvailableDomainEvent &mdash; there is available position(s) in a queue. Subscribers
should be notified. This event is being raised once, when queue is expired, has reached
maximum capacity and position is available after all not paid orders removed.


- QueueExpiredDomainEvent &mdash; queue expired and all orders, which are not paid,
should be removed from it. Subscriptions should be reset.


## API
Don't directly call `queue.Add(order)`, it should be called
strictly from order constructor.

### Raise `QueueExpiredDomainEvent`:
```csharp
    /// <summary>
    /// Makes an attempt to expire queue and raises <see cref="QueueExpiredDomainEvent"/>.
    /// Should be called in some kind of background worker.
    /// </summary>
    /// <returns>true, if event is raised, false otherwise.</returns>
    public bool TryExpire()
    {
        if (Expired)
        {
            Raise(new QueueExpiredDomainEvent(this));
            return true;
        }

        return false;
    }
```

### Raise `PositionAvailableDomainEvent`:
```csharp
    /// <summary>
    /// Raises <see cref="PositionAvailableDomainEvent"/>, if queue is expired
    /// and it's not full by that time.
    /// </summary>
    /// <returns>same queue instance.</returns>
    public QueueEntity NotifyAboutAvailablePosition()
    {
        if (Expired && _maxCapacityReachedOnce)
        {
            Raise(new PositionAvailableDomainEvent(this));
        }

        return this;
    }
```


# User
## Domain events

- UserRegisteredDomainEvent &mdash; user registered. Empty subscription should be assigned to them.


# Subscriber
## API

### Subscribe on newsletter:
```csharp
    /// <summary>
    /// Subscribes order to the newsletter.
    /// </summary>
    /// <param name="order">order to be subscribed.</param>
    /// <returns>subscribed order entity.</returns>
    /// <remarks>returns failure result, when order is already subscribed.</remarks>
    public Result<OrderEntity> Subscribe(OrderEntity order)
    {
        if (_orders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Subscriber.ContainsOrderWithId(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Add(order);
        Queue = order.Queue;

        return order;
    }
```

### Unsubscribe from newsletter:
```csharp
    /// <summary>
    /// Unsubscribes order from the newsletter.
    /// </summary>
    /// <param name="order">order to be unsubscribed.</param>
    /// <returns>unsubscribed order.</returns>
    /// <remarks>returns failure result, when order is not subscribed.</remarks>
    public Result<OrderEntity> Unsubscribe(OrderEntity order)
    {
        if (_orders.Contains(order) is false)
        {
            var exception = new DomainException(DomainErrors.Subscriber.OrderIsNotInSubscription(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Remove(order);

        if (_orders.Count is 0)
        {
            Queue = null;
        }

        return order;
    }
```
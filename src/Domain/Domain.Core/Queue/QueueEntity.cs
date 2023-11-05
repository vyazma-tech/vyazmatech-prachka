using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue.Events;
using Domain.Core.ValueObjects;

namespace Domain.Core.Queue;

public sealed class QueueEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders;

    public QueueEntity(
        Capacity capacity,
        QueueDate creationDate,
        QueueActivityBoundaries activityBoundaries)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(capacity, nameof(capacity), "Capacity should not be null");
        Guard.Against.Null(creationDate, nameof(creationDate), "Creation date should not be null");
        Guard.Against.Null(activityBoundaries, nameof(activityBoundaries), "Activity boundaries should not be null");

        Capacity = capacity;
        ActivityBoundaries = activityBoundaries;
        CreationDate = creationDate.Value;
        _orders = new HashSet<OrderEntity>();
    }

#pragma warning disable CS8618
    private QueueEntity()
#pragma warning restore CS8618
    {
        _orders = new HashSet<OrderEntity>();
    }

    public Capacity Capacity { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public QueueActivityBoundaries ActivityBoundaries { get; }
    public IReadOnlySet<OrderEntity> Items => _orders;

    public bool Expired
        => TimeOnly.FromDateTime(DateTime.UtcNow) >= ActivityBoundaries.ActiveUntil;

    public Result<OrderEntity> Add(OrderEntity order)
    {
        if (_orders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Queue.ContainsOrderWithId(order.Id));
            return new Result<OrderEntity>(exception);
        }

        if (_orders.Count.Equals(Capacity.Value))
        {
            var exception = new DomainException(DomainErrors.Queue.Overfull);
            return new Result<OrderEntity>(exception);
        }

        _orders.Add(order);

        return order;
    }

    public Result<OrderEntity> Remove(OrderEntity order)
    {
        if (_orders.Contains(order) is false)
        {
            var exception = new DomainException(DomainErrors.Queue.OrderIsNotInQueue(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Remove(order);

        return order;
    }

    public Result<QueueEntity> IncreaseCapacity(Capacity newCapacity, DateTime modifiedOnUtc)
    {
        if (newCapacity.Value <= Capacity.Value)
        {
            var exception = new DomainException(DomainErrors.Queue.InvalidNewCapacity);
            return new Result<QueueEntity>(exception);
        }

        Capacity = newCapacity;
        ModifiedOn = modifiedOnUtc;

        Raise(new QueueCapacityIncreasedDomainEvent(this));

        return this;
    }

    public bool TryExpire()
    {
        if (Expired)
        {
            Raise(new QueueExpiredDomainEvent(this));
            return true;
        }

        return false;
    }

    public QueueEntity NotifyAboutAvailablePosition()
    {
        if (Expired && _orders.Count.Equals(Capacity.Value) is false)
        {
            Raise(new PositionAvailableDomainEvent(this));
        }

        return this;
    }
}
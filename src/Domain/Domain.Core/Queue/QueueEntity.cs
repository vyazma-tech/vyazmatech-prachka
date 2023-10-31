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

    public QueueEntity(int capacity, QueueDate creationDate)
        : base(Guid.NewGuid())
    {
        Guard.Against.Zero(capacity, nameof(capacity), "Capacity should be more than 0");
        Guard.Against.Null(creationDate, nameof(creationDate), "Creation date should not be null");

        Capacity = capacity;
        CreationDate = creationDate.Value;
        ModifiedOn = null;
        _orders = new HashSet<OrderEntity>();
    }

    private QueueEntity()
    {
        _orders = new HashSet<OrderEntity>();
    }

    public int Capacity { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public IReadOnlySet<OrderEntity> Items => _orders;

    public Result<OrderEntity> Add(OrderEntity order)
    {
        if (_orders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Queue.ContainsOrderWithId(order.Id));
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

    public Result<QueueEntity> IncreaseCapacity(int newCapacity, DateTime dateTimeUtc)
    {
        if (newCapacity <= Capacity)
        {
            var exception = new DomainException(DomainErrors.Queue.InvalidNewCapacity);
            return new Result<QueueEntity>(exception);
        }

        Capacity = newCapacity;
        ModifiedOn = dateTimeUtc;

        Raise(new QueueCapacityIncreasedDomainEvent(this));

        return this;
    }
}
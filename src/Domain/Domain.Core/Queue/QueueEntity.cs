using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Core.Order;
using Domain.Core.Queue.Events;
using Domain.Core.ValueObjects;
using LanguageExt.Common;

namespace Domain.Core.Queue;

public sealed class QueueEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders = new();

    public QueueEntity(int capacity, QueueDate creationDate)
    {
        Guard.Against.Zero(capacity, nameof(capacity), "Capacity should be more than 0");
        Guard.Against.Null(creationDate, nameof(creationDate), "Creation date should not be null");

        Capacity = capacity;
        CreationDate = creationDate.Value;
        ModifiedOn = null;
    }

    private QueueEntity() { }

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
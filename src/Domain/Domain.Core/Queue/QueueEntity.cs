using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue.Events;
using Domain.Core.ValueObjects;
using Domain.Kernel;

namespace Domain.Core.Queue;

/// <summary>
/// Describes queue entity.
/// </summary>
public class QueueEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders;
    private bool _maxCapacityReachedOnce;
    private bool _queueExpiredOnce;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueueEntity" /> class.
    /// </summary>
    /// <param name="capacity">queue capacity.</param>
    /// <param name="queueDate">queue date, what queue assigned to.</param>
    /// <param name="activityBoundaries">queue activity time. i.e: 1pm - 5pm.</param>
    public QueueEntity(
        Capacity capacity,
        QueueDate queueDate,
        QueueActivityBoundaries activityBoundaries)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(capacity, nameof(capacity), "Capacity should not be null");
        Guard.Against.Null(queueDate, nameof(queueDate), "Creation date should not be null");
        Guard.Against.Null(activityBoundaries, nameof(activityBoundaries), "Activity boundaries should not be null");

        Capacity = capacity;
        ActivityBoundaries = activityBoundaries;
        CreationDate = queueDate.Value;
        _orders = new HashSet<OrderEntity>();
    }

#pragma warning disable CS8618
    protected QueueEntity()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// Gets current capacity.
    /// </summary>
    public virtual Capacity Capacity { get; private set; }

    /// <summary>
    /// Gets time range for a queue activity.
    /// </summary>
    public virtual QueueActivityBoundaries ActivityBoundaries { get; private set; }

    /// <summary>
    /// Gets orders, that currently in the queue.
    /// </summary>
    public virtual IReadOnlySet<OrderEntity> Items => _orders;

    /// <summary>
    /// Gets a value indicating whether queue expired or not.
    /// </summary>
    public bool Expired => TimeOnly.FromDateTime(DateTime.UtcNow) >= ActivityBoundaries.ActiveUntil;

    /// <summary>
    /// Gets date, what queue is assigned to.
    /// </summary>
    public DateOnly CreationDate { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public DateTime? ModifiedOn { get; private set; }

    /// <summary>
    /// Add order into a queue. Should <b>never</b> be called from queue instance,
    /// because it does not update order queue reference.
    /// </summary>
    /// <param name="order">order to be added.</param>
    /// <returns>added order instance.</returns>
    /// <remarks>returns failure result, when order is already in a queue.</remarks>
    /// <remarks>returns failure result, when order is being enqueued into full queue.</remarks>
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

        if (Expired)
        {
            var exception = new DomainException(DomainErrors.Queue.Expired);
            return new Result<OrderEntity>(exception);
        }

        _orders.Add(order);
        if (_orders.Count.Equals(Capacity.Value) is true)
            _maxCapacityReachedOnce = true;

        return order;
    }

    /// <summary>
    /// Removes order from a queue.
    /// </summary>
    /// <param name="order">order to be removed.</param>
    /// <returns>removed order entity.</returns>
    /// <remarks>return failure result, when order is not in queue.</remarks>
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

    /// <summary>
    /// Increases queue capacity.
    /// </summary>
    /// <param name="newCapacity">new capacity value.</param>
    /// <param name="modifiedOnUtc">modification utc date.</param>
    /// <returns>same queue instance.</returns>
    /// <remarks>returns failure result, when new capacity is invalid.</remarks>
    public Result<QueueEntity> IncreaseCapacity(Capacity newCapacity, DateTime modifiedOnUtc)
    {
        if (newCapacity.Value <= Capacity.Value)
        {
            var exception = new DomainException(DomainErrors.Queue.InvalidNewCapacity);
            return new Result<QueueEntity>(exception);
        }

        Capacity = newCapacity;
        ModifiedOn = modifiedOnUtc;

        return this;
    }

    /// <summary>
    /// Change queue activity boundaries.
    /// </summary>
    /// <param name="activityBoundaries">new activity boundaries value.</param>
    /// <param name="modifiedOnUtc">modification utc date.</param>
    /// <returns>same queue instance.</returns>
    /// <remarks>returns failure result, when new activity boundaries is invalid.</remarks>
    public Result<QueueEntity> ChangeActivityBoundaries(
        QueueActivityBoundaries activityBoundaries, DateTime modifiedOnUtc)
    {
        if (activityBoundaries == ActivityBoundaries)
        {
            var exception = new DomainException(DomainErrors.Queue.InvalidNewActivityBoundaries);
            return new Result<QueueEntity>(exception);
        }

        ActivityBoundaries = activityBoundaries;
        ModifiedOn = modifiedOnUtc;

        return this;
    }

    /// <summary>
    /// Makes an attempt to expire queue and raises <see cref="QueueExpiredDomainEvent" />.
    /// Should be called in some kind of background worker.
    /// </summary>
    /// <returns>true, if event is raised, false otherwise.</returns>
    public bool TryExpire()
    {
        if (Expired && _queueExpiredOnce is false)
        {
            Raise(new QueueExpiredDomainEvent(this));
            _queueExpiredOnce = true;

            return _queueExpiredOnce;
        }

        return false;
    }

    /// <summary>
    /// Raises <see cref="PositionAvailableDomainEvent" />, if queue is expired
    /// and it's not full by that time.
    /// </summary>
    /// <returns>true, if event is raised, false otherwise.</returns>
    public bool TryNotifyAboutAvailablePosition()
    {
        if (_queueExpiredOnce && _maxCapacityReachedOnce)
        {
            Raise(new PositionAvailableDomainEvent(this));
            return true;
        }

        return false;
    }
}
using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue.Events;
using Domain.Core.ValueObjects;
using Domain.Kernel;

namespace Domain.Core.Queue;

/// <summary>
/// Describes queue entity.
/// </summary>
public sealed class QueueEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueueEntity" /> class.
    /// </summary>
    /// <param name="id">queue id.</param>
    /// <param name="capacity">queue capacity.</param>
    /// <param name="queueDate">queue date, what queue assigned to.</param>
    /// <param name="activityBoundaries">queue activity time. i.e: 1pm - 5pm.</param>
    /// <param name="state">current queue state.</param>
    /// <param name="maxCapacityReachedOnce">max capacity reached.</param>
    /// <param name="modifiedOn">queue modification date.</param>
    public QueueEntity(
        Guid id,
        Capacity capacity,
        QueueDate queueDate,
        QueueActivityBoundaries activityBoundaries,
        QueueState state,
        bool maxCapacityReachedOnce = false,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        Guard.Against.Null(capacity, nameof(capacity), "Capacity should not be null");
        Guard.Against.Null(queueDate, nameof(queueDate), "Creation date should not be null");
        Guard.Against.Null(activityBoundaries, nameof(activityBoundaries), "Activity boundaries should not be null");

        Capacity = capacity;
        ActivityBoundaries = activityBoundaries;
        State = state;
        MaxCapacityReachedOnce = maxCapacityReachedOnce;
        CreationDate = queueDate.Value;
        ModifiedOn = modifiedOn;
        _orders = new HashSet<OrderEntity>();
    }

    /// <summary>
    /// Gets current capacity.
    /// </summary>
    public Capacity Capacity { get; private set; }

    /// <summary>
    /// Gets time range for a queue activity.
    /// </summary>
    public QueueActivityBoundaries ActivityBoundaries { get; private set; }

    /// <summary>
    /// Gets a value indicating whether max capacity reached during activity time.
    /// </summary>
    public bool MaxCapacityReachedOnce { get; private set; }

    /// <summary>
    /// Gets queue current state.
    /// </summary>
    public QueueState State { get; private set; }

    /// <summary>
    /// Gets orders, that currently in the queue.
    /// </summary>
    public IReadOnlySet<OrderEntity> Items => _orders;

    /// <summary>
    /// Gets date, what queue is assigned to.
    /// </summary>
    public DateOnly CreationDate { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public SpbDateTime? ModifiedOn { get; private set; }

    /// <summary>
    /// Add order into a queue. Should <b>never</b> be called from queue instance,
    /// because it does not update order queue reference.
    /// </summary>
    /// <param name="order">order to be added.</param>
    /// <param name="currentTimeUtc">current date time in utc format.</param>
    /// <returns>added order instance.</returns>
    /// <remarks>returns failure result, when order is already in a queue.</remarks>
    /// <remarks>returns failure result, when order is being enqueued into full queue.</remarks>
    public Result<OrderEntity> Add(OrderEntity order, SpbDateTime currentTimeUtc)
    {
        if (_orders.Contains(order))
        {
            return new Result<OrderEntity>(DomainErrors.Queue.ContainsOrderWithId(order.Id));
        }

        if (_orders.Count.Equals(Capacity.Value))
        {
            return new Result<OrderEntity>(DomainErrors.Queue.Overfull);
        }

        if (Expired(currentTimeUtc))
        {
            return new Result<OrderEntity>(DomainErrors.Queue.Expired);
        }

        _orders.Add(order);
        if (_orders.Count.Equals(Capacity.Value) is true)
            MaxCapacityReachedOnce = true;

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
            return new Result<OrderEntity>(DomainErrors.Queue.OrderIsNotInQueue(order.Id));
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
    public Result<QueueEntity> IncreaseCapacity(Capacity newCapacity, SpbDateTime modifiedOnUtc)
    {
        if (newCapacity.Value <= Capacity.Value)
        {
            return new Result<QueueEntity>(DomainErrors.Queue.InvalidNewCapacity);
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
        QueueActivityBoundaries activityBoundaries, SpbDateTime modifiedOnUtc)
    {
        if (activityBoundaries == ActivityBoundaries)
        {
            return new Result<QueueEntity>(DomainErrors.Queue.InvalidNewActivityBoundaries);
        }

        ActivityBoundaries = activityBoundaries;
        ModifiedOn = modifiedOnUtc;

        return this;
    }

    /// <summary>
    /// Makes an attempt to expire queue and raises <see cref="QueueExpiredDomainEvent" />.
    /// Should be called in some kind of background worker.
    /// </summary>
    /// <param name="currentTimeUtc">current date time in utc format.</param>
    /// <returns>true, if event is raised, false otherwise.</returns>
    public bool TryExpire(SpbDateTime currentTimeUtc)
    {
        if (Expired(currentTimeUtc) && State == QueueState.Active)
        {
            Raise(new QueueExpiredDomainEvent(this));
            State = QueueState.Expired;
            ModifiedOn = currentTimeUtc;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Raises <see cref="PositionAvailableDomainEvent" />, if queue is expired
    /// and it's not full by that time.
    /// </summary>
    /// <param name="currentDateTimeUtc">current date time in utc format.</param>
    /// <returns>true, if event is raised, false otherwise.</returns>
    public bool TryNotifyAboutAvailablePosition(SpbDateTime currentDateTimeUtc)
    {
        if (State == QueueState.Expired && MaxCapacityReachedOnce)
        {
            Raise(new PositionAvailableDomainEvent(this));
            ModifiedOn = currentDateTimeUtc;
            MaxCapacityReachedOnce = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Makes an attempt to activate queue.
    /// </summary>
    /// <param name="currentDateTimeUtc">current date time in utc format.</param>
    /// <returns>true, if queue is activated, false otherwise.</returns>
    public bool TryActivate(SpbDateTime currentDateTimeUtc)
    {
        if (currentDateTimeUtc.AsTimeOnly() >= ActivityBoundaries.ActiveFrom &&
            State == QueueState.Prepared)
        {
            State = QueueState.Active;
            ModifiedOn = currentDateTimeUtc;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets a value indicating whether queue expired or not.
    /// </summary>
    private bool Expired(SpbDateTime currentDateTimeUtc)
    {
        var dateNow = currentDateTimeUtc.AsDateOnly();

        return
            (currentDateTimeUtc.AsTimeOnly() >= ActivityBoundaries.ActiveUntil
             && dateNow == CreationDate)
            || dateNow > CreationDate;
    }
}
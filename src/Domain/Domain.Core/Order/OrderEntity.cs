using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order.Events;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Kernel;

namespace Domain.Core.Order;

/// <summary>
/// Describes order entity.
/// </summary>
public sealed class OrderEntity : Entity, IAuditableEntity
{
    private OrderEntity(
        Guid id,
        UserEntity user,
        QueueEntity queue,
        OrderStatus status,
        SpbDateTime creationDateTimeUtc,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in order.");
        Guard.Against.Null(queue, nameof(queue), "Queue should not be null in order.");
        Guard.Against.Null(creationDateTimeUtc, nameof(creationDateTimeUtc), "Creation date should not be null in order.");

        User = user;
        Queue = queue;
        CreationDateTime = creationDateTimeUtc;
        Status = status;
        ModifiedOn = modifiedOn;
    }

    /// <inheritdoc cref="UserEntity" />
    public UserEntity User { get; }

    /// <inheritdoc cref="QueueEntity" />
    public QueueEntity Queue { get; private set; }

    /// <summary>
    /// Gets a value indicating order status.
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Gets order creation date.
    /// </summary>
    public DateOnly CreationDate => CreationDateTime.AsDateOnly();

    /// <summary>
    /// Gets order creation date time utc.
    /// </summary>
    public SpbDateTime CreationDateTime { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public SpbDateTime? ModifiedOn { get; private set; }

    /// <summary>
    /// Validates provided data for order and constructs order instance, when data is valid.
    /// </summary>
    /// <param name="id">order id.</param>
    /// <param name="user">order issuer.</param>
    /// <param name="queue">queue of the order.</param>
    /// <param name="status">current order status.</param>
    /// <param name="creationDateUtc">order creation date.</param>
    /// <param name="modifiedOn">order modification date.</param>
    /// <returns>order instance.</returns>
    /// <remarks>returns failure result, when order is being enqueued into full queue.</remarks>
    /// <remarks>returns failure result, when order is already in the queue.</remarks>
    public static Result<OrderEntity> Create(
        Guid id,
        UserEntity user,
        QueueEntity queue,
        OrderStatus status,
        SpbDateTime creationDateUtc,
        SpbDateTime? modifiedOn = null)
    {
        var order = new OrderEntity(id, user, queue, status, creationDateUtc, modifiedOn);

        Result<OrderEntity> entranceResult = queue.Add(order, creationDateUtc);

        if (entranceResult.IsFaulted)
        {
            return entranceResult;
        }

        return order;
    }

    /// <summary>
    /// Pays order and raises <see cref="OrderPaidDomainEvent" />.
    /// </summary>
    /// <param name="dateTimeUtc">payment utc date.</param>
    /// <returns>same order instance.</returns>
    /// <remarks>returns failure result, when order is already is paid.</remarks>
    public Result<OrderEntity> MakePayment(SpbDateTime dateTimeUtc)
    {
        if (Status == OrderStatus.Paid)
        {
            return new Result<OrderEntity>(DomainErrors.Order.AlreadyPaid);
        }

        Status = OrderStatus.Paid;
        ModifiedOn = dateTimeUtc;
        Raise(new OrderPaidDomainEvent(this));

        return this;
    }

    /// <summary>
    /// Makes order ready and raises <see cref="OrderReadyDomainEvent" />.
    /// </summary>
    /// <param name="dateTimeUtc">ready utc date.</param>
    /// <returns>same order instance.</returns>
    /// <remarks>returns failure result, when order is already marked as ready.</remarks>
    public Result<OrderEntity> MakeReady(SpbDateTime dateTimeUtc)
    {
        if (Status == OrderStatus.Ready)
        {
            return new Result<OrderEntity>(DomainErrors.Order.IsReady);
        }

        Status = OrderStatus.Ready;
        ModifiedOn = dateTimeUtc;
        Raise(new OrderReadyDomainEvent(this));

        return this;
    }

    /// <summary>
    /// Sets new queue for an order.
    /// </summary>
    /// <param name="queue">queue, which order should be assigned to.</param>
    /// <param name="dateTimeUtc">modification date.</param>
    /// <returns>same order instance.</returns>
    public OrderEntity Prolong(QueueEntity queue, SpbDateTime dateTimeUtc)
    {
        ModifiedOn = dateTimeUtc;
        Queue = queue;
        Status = OrderStatus.Prolonged;

        return this;
    }
}
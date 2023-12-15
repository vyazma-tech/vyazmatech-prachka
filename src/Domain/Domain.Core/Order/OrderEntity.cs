using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order.Events;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Kernel;

namespace Domain.Core.Order;

/// <summary>
/// Describes order entity.
/// </summary>
public class OrderEntity : Entity, IAuditableEntity
{
#pragma warning disable CS8618
    protected OrderEntity() { }
#pragma warning restore CS8618

    private OrderEntity(UserEntity user, QueueEntity queue, DateOnly creationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in order.");
        Guard.Against.Null(queue, nameof(queue), "Queue should not be null in order.");
        Guard.Against.Null(creationDateUtc, nameof(creationDateUtc), "Creation date should not be null in order.");

        User = user;
        Queue = queue;
        CreationDate = creationDateUtc;
    }

    /// <inheritdoc cref="UserEntity" />
    public virtual UserEntity User { get; }

    /// <inheritdoc cref="QueueEntity" />
    public virtual QueueEntity Queue { get; private set; }

    /// <summary>
    /// Gets a value indicating whether order paid or not.
    /// </summary>
    public bool Paid { get; private set; }

    /// <summary>
    /// Gets a value indicating whether order ready or not.
    /// </summary>
    public bool Ready { get; private set; }

    /// <summary>
    /// Gets order creation date.
    /// </summary>
    public DateOnly CreationDate { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public DateTime? ModifiedOn { get; private set; }

    /// <summary>
    /// Validates provided data for order and constructs order instance, when data is valid.
    /// </summary>
    /// <param name="user">order issuer.</param>
    /// <param name="queue">queue of the order.</param>
    /// <param name="creationDateUtc">order creation date.</param>
    /// <returns>order instance.</returns>
    /// <remarks>returns failure result, when order is being enqueued into full queue.</remarks>
    /// <remarks>returns failure result, when order is already in the queue.</remarks>
    public static Result<OrderEntity> Create(UserEntity user, QueueEntity queue, DateOnly creationDateUtc)
    {
        var order = new OrderEntity(user, queue, creationDateUtc);
        Result<OrderEntity> entranceResult = queue.Add(order);

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
    public Result<OrderEntity> MakePayment(DateTime dateTimeUtc)
    {
        if (Paid)
        {
            var exception = new DomainException(DomainErrors.Order.AlreadyPaid);
            return new Result<OrderEntity>(exception);
        }

        Paid = true;
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
    public Result<OrderEntity> MakeReady(DateTime dateTimeUtc)
    {
        if (Ready)
        {
            var exception = new DomainException(DomainErrors.Order.IsReady);
            return new Result<OrderEntity>(exception);
        }

        Ready = true;
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
    public OrderEntity Prolong(QueueEntity queue, DateTime dateTimeUtc)
    {
        ModifiedOn = dateTimeUtc;
        Queue = queue;

        return this;
    }
}
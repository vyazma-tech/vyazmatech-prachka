using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Order.Events;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.User;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Order;

public sealed class OrderEntity : Entity, IAuditableEntity
{
    public OrderEntity(
        Guid id,
        Guid queueId,
        UserInfo user,
        OrderStatus status,
        DateTime creationDateTimeUtc,
        DateTime? modifiedOn = null)
        : base(id)
    {
        Queue = queueId;
        User = user;
        Status = status;
        CreationDateTime = creationDateTimeUtc;
        ModifiedOnUtc = modifiedOn;
    }

    public OrderInfo Info => new(Id, User, Queue, Status);

    public Guid Queue { get; private set; }

    public UserInfo User { get; }

    public OrderStatus Status { get; private set; }

    public DateOnly CreationDate => CreationDateTime.AsDateOnly();

    public DateTime CreationDateTime { get; }

    public DateTime? ModifiedOnUtc { get; private set; }

    /// <summary>
    /// Pays order and raises <see cref="OrderPaidDomainEvent" />.
    /// </summary>
    /// <param name="dateTimeUtc">payment utc date.</param>
    /// <returns>same order instance.</returns>
    /// <remarks>returns failure result, when order is already is paid.</remarks>
    public OrderEntity MakePayment(DateTime dateTimeUtc)
    {
        if (Status == OrderStatus.Paid)
            throw new DomainInvalidOperationException(DomainErrors.Order.AlreadyPaid);

        Status = OrderStatus.Paid;
        ModifiedOnUtc = dateTimeUtc;
        Raise(new OrderPaidDomainEvent(this));

        return this;
    }

    /// <summary>
    /// Makes order ready and raises <see cref="OrderReadyDomainEvent" />.
    /// </summary>
    /// <param name="dateTimeUtc">ready utc date.</param>
    /// <returns>same order instance.</returns>
    /// <remarks>returns failure result, when order is already marked as ready.</remarks>
    public OrderEntity MakeReady(DateTime dateTimeUtc)
    {
        if (Status == OrderStatus.Ready)
            throw new DomainInvalidOperationException(DomainErrors.Order.IsReady);

        Status = OrderStatus.Ready;
        ModifiedOnUtc = dateTimeUtc;
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
        // TODO: remove from current queue
        queue.Add(this, dateTimeUtc);

        ModifiedOnUtc = dateTimeUtc;
        Queue = queue.Id;
        Status = OrderStatus.Prolonged;

        return this;
    }
}
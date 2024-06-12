using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Orders;

public sealed class Order : Entity, IAuditableEntity
{
    public Order(
        Guid id,
        Queue queue,
        User user,
        OrderStatus status,
        DateTime creationDateTimeUtc,
        DateTime? modifiedOn = null)
        : base(id)
    {
        Queue = queue;
        User = user;
        Status = status;
        CreationDateTime = creationDateTimeUtc;
        ModifiedOnUtc = modifiedOn;
    }

    public Queue Queue { get; private set; }

    public User User { get; }

    public OrderStatus Status { get; private set; }

    public DateOnly CreationDate => CreationDateTime.AsDateOnly();

    public DateTime CreationDateTime { get; }

    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Pays order and raises <see cref="OrderPaidDomainEvent" />.
    /// </summary>
    /// <remarks>returns failure result, when order is already is paid.</remarks>
    public void MakePayment()
    {
        if (Status == OrderStatus.Paid)
            throw new DomainInvalidOperationException(DomainErrors.Order.AlreadyPaid);

        Status = OrderStatus.Paid;
        Raise(new OrderPaidDomainEvent(this));
    }

    /// <summary>
    /// Makes order ready and raises <see cref="OrderReadyDomainEvent" />.
    /// </summary>
    /// <remarks>returns failure result, when order is already marked as ready.</remarks>
    public void MakeReady()
    {
        if (Status == OrderStatus.Ready)
            throw new DomainInvalidOperationException(DomainErrors.Order.IsReady);

        Status = OrderStatus.Ready;
        Raise(new OrderReadyDomainEvent(this));
    }

    /// <summary>
    /// Sets new queue for an order.
    /// </summary>
    /// <param name="queue">queue, which order should be assigned to.</param>
    /// <param name="dateTimeUtc">modification date.</param>
    public void Prolong(Queue queue, DateTime dateTimeUtc)
    {
        Queue.Remove(this);
        queue.Add(this, dateTimeUtc);

        Status = OrderStatus.Prolonged;
    }
}
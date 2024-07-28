using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Domain.Core.Orders;

public sealed class Order : Entity, IAuditableEntity
{
    private Order() { }

    public Order(
        Guid id,
        Queue queue,
        User user,
        OrderStatus status,
        DateTime creationDateTimeUtc,
        Price price,
        string? comment = null,
        DateTime? modifiedOn = null)
        : base(id)
    {
        Queue = queue;
        User = user;
        Status = status;
        CreationDateTime = creationDateTimeUtc;
        CreationDate = creationDateTimeUtc.AsDateOnly();
        Price = price;
        Comment = comment;
        ModifiedOnUtc = modifiedOn;
    }

    public Queue Queue { get; private set; }

    public User User { get; }

    public OrderStatus Status { get; private set; }

    public DateOnly CreationDate { get; }

    public DateTime CreationDateTime { get; }

    public DateTime? ModifiedOnUtc { get; }

    public Price Price { get; private set; }

    public string? Comment { get; private set; }

    /// <summary>
    /// Pays order and raises <see cref="OrderPaidDomainEvent" />.
    /// </summary>
    /// <remarks>returns failure result, when order is already is paid.</remarks>
    public void MakePayment(double price, string comment)
    {
        if (Status == OrderStatus.Paid)
            throw new DomainInvalidOperationException(DomainErrors.Order.AlreadyPaid);

        var model = Price.Create(price);

        Status = OrderStatus.Paid;
        Price = model;
        Comment = comment;
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
        Comment = $"Ваш заказ переведен в статус Готов";
        Raise(new OrderReadyDomainEvent(this));
    }

    /// <summary>
    /// Sets new queue for an order.
    /// </summary>
    /// <param name="queue">queue, which order should be assigned to.</param>
    public void ProlongInto(Queue queue)
    {
        Queue.Remove(this);
        queue.BulkInsert([this]);

        Comment = "Заказ переведен в другую очередь, потому что у сотрудников прачечной не хватило времени выполнить заказ. Следите за статусом заказа";
        Status = OrderStatus.Prolonged;
    }

    /// <summary>
    /// Removes order from current queue. Should
    /// be only used for not paid order dismissal
    /// </summary>
    public void Dismiss()
    {
        if (Status == OrderStatus.New)
            Queue.Remove(this);
    }
}
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
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
        SpbDateTime creationDateTimeUtc,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        Queue = queueId;
        User = user;
        Status = status;
        CreationDateTime = creationDateTimeUtc;
        ModifiedOn = modifiedOn;
    }

    public OrderInfo Info => new OrderInfo(Id, User, Queue, Status);

    public Guid Queue { get; private set; }

    public UserInfo User { get; }

    public OrderStatus Status { get; private set; }

    public DateOnly CreationDate => CreationDateTime.AsDateOnly();

    public SpbDateTime CreationDateTime { get; }

    public SpbDateTime? ModifiedOn { get; private set; }

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
    public Result<OrderEntity> Prolong(QueueEntity queue, SpbDateTime dateTimeUtc)
    {
        Result<OrderEntity> entranceResult = queue.Add(this, dateTimeUtc);

        if (entranceResult.IsFaulted)
        {
            return new Result<OrderEntity>(entranceResult.Error);
        }

        ModifiedOn = dateTimeUtc;
        Queue = queue.Id;
        Status = OrderStatus.Prolonged;

        return this;
    }
}
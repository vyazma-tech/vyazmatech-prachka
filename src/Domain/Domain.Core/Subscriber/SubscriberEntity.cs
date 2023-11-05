﻿using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;

namespace Domain.Core.Subscriber;

/// <summary>
/// Describes subscriber entity.
/// </summary>
public sealed class SubscriberEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriberEntity"/> class.
    /// </summary>
    /// <param name="user">subscribed user.</param>
    /// <param name="creationDateUtc">subscription creation utc date.</param>
    public SubscriberEntity(UserEntity user, DateTime creationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in subscription.");
        Guard.Against.Null(creationDateUtc, nameof(creationDateUtc), "Creation date should not be null in subscription.");

        User = user;
        CreationDate = creationDateUtc;
        _orders = new HashSet<OrderEntity>();
    }

#pragma warning disable CS8618
    private SubscriberEntity()
#pragma warning restore CS8618
    {
        _orders = new HashSet<OrderEntity>();
    }

    /// <summary>
    /// Gets orders, that are subscribed to the newsletter.
    /// </summary>
    public IReadOnlySet<OrderEntity> Orders => _orders;

    /// <summary>
    /// Gets subscription creation date.
    /// </summary>
    public DateTime CreationDate { get; private set; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public DateTime? ModifiedOn { get; private set; }

    /// <summary>
    /// Gets user, who subscription is assigned to.
    /// </summary>
    public UserEntity User { get; private set; }

    /// <summary>
    /// Gets queue, orders from which are subscribed to the newsletter.
    /// </summary>
    public QueueEntity? Queue { get; private set; }

    /// <summary>
    /// Subscribes order to the newsletter.
    /// </summary>
    /// <param name="order">order to be subscribed.</param>
    /// <returns>subscribed order entity.</returns>
    /// <remarks>returns failure result, when order is already subscribed.</remarks>
    public Result<OrderEntity> Subscribe(OrderEntity order)
    {
        if (_orders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Subscriber.ContainsOrderWithId(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Add(order);
        Queue = order.Queue;

        return order;
    }

    /// <summary>
    /// Unsubscribes order from the newsletter.
    /// </summary>
    /// <param name="order">order to be unsubscribed.</param>
    /// <returns>unsubscribed order.</returns>
    /// <remarks>returns failure result, when order is not subscribed.</remarks>
    public Result<OrderEntity> Unsubscribe(OrderEntity order)
    {
        if (_orders.Contains(order) is false)
        {
            var exception = new DomainException(DomainErrors.Subscriber.OrderIsNotInSubscription(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Remove(order);

        if (_orders.Count is 0)
        {
            Queue = null;
        }

        return order;
    }
}
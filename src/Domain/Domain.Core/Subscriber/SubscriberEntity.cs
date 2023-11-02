using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscriber.Events;
using Domain.Core.User;

namespace Domain.Core.Subscriber;

public sealed class SubscriberEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders;

    public SubscriberEntity(UserEntity user, DateTime creationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in subscription.");
        Guard.Against.Null(creationDateUtc, nameof(creationDateUtc), "Creation date should not be null in subscription.");

        User = user;
        CreationDate = creationDateUtc;
        _orders = new HashSet<OrderEntity>();
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private SubscriberEntity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _orders = new HashSet<OrderEntity>();
    }

    public IReadOnlySet<OrderEntity> Orders => _orders;
    public DateTime CreationDate { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public UserEntity User { get; private set; }
    public QueueEntity? Queue { get; private set; }

    public Result<OrderEntity> Subscribe(OrderEntity order)
    {
        if (_orders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Subscriber.ContainsOrderWithId(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Add(order);
        Queue = order.Queue;
        Raise(new UserSubscribedDomainEvent(this));

        return order;
    }

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
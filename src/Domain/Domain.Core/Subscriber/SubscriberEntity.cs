using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Core.Order;
using Domain.Core.Subscriber.Events;
using Domain.Core.ValueObjects;
using LanguageExt.Common;

namespace Domain.Core.Subscriber;

public class SubscriberEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderEntity> _orders;

    public SubscriberEntity(SubscriberDate creationDate, Guid userId, Guid queueId)
        : base(Guid.NewGuid())
    {
        CreationDate = creationDate.Value;
        ModifiedOn = null;
        UserId = userId;
        QueueId = queueId;
        _orders = new HashSet<OrderEntity>();

        Raise(new UserSubscribedDomainEvent(this));
    }

    private SubscriberEntity()
    {
        _orders = new HashSet<OrderEntity>();
    }

    public IReadOnlySet<OrderEntity> Orders => _orders;

    public DateTime CreationDate { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public Guid UserId { get; private set; }
    public Guid QueueId { get; private set; }

    public Result<OrderEntity> Add(OrderEntity order)
    {
        if (_orders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Subscriber.ContainsOrderWithId(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Add(order);

        return order;
    }

    public Result<OrderEntity> Remove(OrderEntity order)
    {
        if (_orders.Contains(order) is false)
        {
            var exception = new DomainException(DomainErrors.Subscriber.OrderIsNotInSubscription(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _orders.Remove(order);

        return order;
    }
}
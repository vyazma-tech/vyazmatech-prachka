using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;

namespace Domain.Core.Subscription;

public class OrderSubscriptionEntity : SubscriptionEntity
{
    private readonly List<OrderEntity> _subscribedOrders;

    public OrderSubscriptionEntity(UserEntity user, DateOnly creationDateUtc)
        : base(user, creationDateUtc)
    {
        _subscribedOrders = new List<OrderEntity>();
    }

#pragma warning disable CS8618
    protected OrderSubscriptionEntity() { }
#pragma warning restore CS8618

    public virtual IReadOnlyCollection<OrderEntity> SubscribedOrders => _subscribedOrders;

    /// <summary>
    /// Subscribes order to the newsletter.
    /// </summary>
    /// <param name="order">order to be subscribed.</param>
    /// <returns>subscribed order entity.</returns>
    /// <remarks>returns failure result, when order is already subscribed.</remarks>
    public Result<OrderEntity> Subscribe(OrderEntity order)
    {
        if (_subscribedOrders.Contains(order))
        {
            var exception = new DomainException(DomainErrors.Subscription.ContainsOrderWithId(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _subscribedOrders.Add(order);

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
        if (_subscribedOrders.Contains(order) is false)
        {
            var exception = new DomainException(DomainErrors.Subscription.OrderIsNotInSubscription(order.Id));
            return new Result<OrderEntity>(exception);
        }

        _subscribedOrders.Remove(order);

        return order;
    }
}
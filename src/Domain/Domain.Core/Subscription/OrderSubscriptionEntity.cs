using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;

namespace Domain.Core.Subscription;

public sealed class OrderSubscriptionEntity : SubscriptionEntity
{
    private readonly List<OrderEntity> _subscribedOrders;

    public OrderSubscriptionEntity(
        Guid id,
        UserEntity user,
        DateOnly creationDateUtc,
        SpbDateTime? modifiedOn = null)
        : base(id, user, creationDateUtc, modifiedOn)
    {
        _subscribedOrders = new List<OrderEntity>();
    }

    public IReadOnlyCollection<OrderEntity> SubscribedOrders => _subscribedOrders;

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
            return new Result<OrderEntity>(DomainErrors.Subscription.ContainsOrderWithId(order.Id));
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
            return new Result<OrderEntity>(DomainErrors.Subscription.OrderIsNotInSubscription(order.Id));
        }

        _subscribedOrders.Remove(order);

        return order;
    }
}
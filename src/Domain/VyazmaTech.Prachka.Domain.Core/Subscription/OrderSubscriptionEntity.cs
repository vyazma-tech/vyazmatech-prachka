using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;

namespace VyazmaTech.Prachka.Domain.Core.Subscription;

public sealed class OrderSubscriptionEntity : SubscriptionEntity
{
    private readonly HashSet<Guid> _orderIds;

    public OrderSubscriptionEntity(
        Guid id,
        Guid user,
        DateOnly creationDateUtc,
        HashSet<Guid> orderIds,
        DateTime? modifiedOn = null)
        : base(id, user, creationDateUtc, modifiedOn)
    {
        _orderIds = orderIds;
    }

    public IReadOnlyCollection<Guid> SubscribedOrders => _orderIds;

    public Result<OrderEntity> Subscribe(OrderEntity order)
    {
        if (_orderIds.Contains(order.Id))
        {
            return new Result<OrderEntity>(DomainErrors.Subscription.ContainsOrderWithId(order.Id));
        }

        _orderIds.Add(order.Id);

        return order;
    }

    public Result<OrderEntity> Unsubscribe(OrderEntity order)
    {
        if (_orderIds.Contains(order.Id) is false)
        {
            return new Result<OrderEntity>(DomainErrors.Subscription.OrderIsNotInSubscription(order.Id));
        }

        _orderIds.Remove(order.Id);

        return order;
    }
}
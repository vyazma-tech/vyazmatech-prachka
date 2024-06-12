using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

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

    public void Subscribe(Guid id)
    {
        if (_orderIds.Contains(id))
            throw new DomainInvalidOperationException(DomainErrors.Subscription.ContainsOrderWithId(id));

        _orderIds.Add(id);
    }

    public void Unsubscribe(Guid id)
    {
        if (_orderIds.Contains(id) is false)
            throw new DomainInvalidOperationException(DomainErrors.Subscription.OrderIsNotInSubscription(id));

        _orderIds.Remove(id);
    }
}
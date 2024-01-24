using Application.DataAccess.Contracts.Querying.OrderSubscription;
using Domain.Core.Subscription;

namespace Application.DataAccess.Contracts.Repositories;

public interface IOrderSubscriptionRepository
{
    IAsyncEnumerable<OrderSubscriptionEntity> QueryAsync(
        OrderSubscriptionQuery specification,
        CancellationToken cancellationToken);

    void Insert(OrderSubscriptionEntity subscription);

    void Update(OrderSubscriptionEntity subscription);
}
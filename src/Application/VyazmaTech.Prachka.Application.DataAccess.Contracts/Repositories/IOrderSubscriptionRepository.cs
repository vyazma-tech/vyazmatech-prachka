using VyazmaTech.Prachka.Application.Abstractions.Querying.OrderSubscription;
using VyazmaTech.Prachka.Domain.Core.Subscription;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IOrderSubscriptionRepository
{
    IAsyncEnumerable<OrderSubscriptionEntity> QueryAsync(
        OrderSubscriptionQuery specification,
        CancellationToken cancellationToken);

    void Insert(OrderSubscriptionEntity subscription);

    void Update(OrderSubscriptionEntity subscription);
}
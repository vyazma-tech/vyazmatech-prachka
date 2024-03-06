using VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription;
using VyazmaTech.Prachka.Domain.Core.Subscription;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IQueueSubscriptionRepository
{
    IAsyncEnumerable<QueueSubscriptionEntity> QueryAsync(
        QueueSubscriptionQuery specification,
        CancellationToken cancellationToken);

    void Insert(QueueSubscriptionEntity subscription);

    void Update(QueueSubscriptionEntity subscription);
}
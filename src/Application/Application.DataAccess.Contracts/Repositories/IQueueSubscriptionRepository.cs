using Application.DataAccess.Contracts.Querying.QueueSubscription;
using Domain.Core.Subscription;

namespace Application.DataAccess.Contracts.Repositories;

public interface IQueueSubscriptionRepository
{
    IAsyncEnumerable<QueueSubscriptionEntity> QueryAsync(
        QueueSubscriptionQuery specification,
        CancellationToken cancellationToken);

    void Insert(QueueSubscriptionEntity subscription);

    void Update(QueueSubscriptionEntity subscription);
}
using Domain.Common.Result;
using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IQueueSubscriptionRepository
{
    Task<Result<QueueSubscriptionEntity>> FindByAsync(
        Specification<QueueSubscriptionModel> specification,
        CancellationToken cancellationToken);

    void Insert(QueueSubscriptionEntity subscription);

    void Update(QueueSubscriptionEntity subscription);
}
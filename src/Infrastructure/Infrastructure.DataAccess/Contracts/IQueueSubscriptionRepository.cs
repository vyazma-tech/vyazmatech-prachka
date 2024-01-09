using Domain.Common.Result;
using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IQueueSubscriptionRepository
{
    Task<Result<SubscriptionEntity>> FindByAsync(
        Specification<QueueSubscriptionModel> specification,
        CancellationToken cancellationToken);

    void Insert(SubscriptionEntity subscription);

    void Update(SubscriptionEntity subscription);
}
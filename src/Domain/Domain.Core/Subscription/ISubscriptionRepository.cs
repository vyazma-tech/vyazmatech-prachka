using Domain.Common.Result;
using Domain.Kernel;

namespace Domain.Core.Subscription;

public interface ISubscriptionRepository
{
    Task<Result<SubscriptionEntity>> FindByAsync(
        Specification<SubscriptionEntity> specification,
        CancellationToken cancellationToken);

    void Insert(SubscriptionEntity subscription);

    void Update(SubscriptionEntity subscription);
}
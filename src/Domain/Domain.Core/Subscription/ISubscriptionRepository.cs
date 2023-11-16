using Domain.Common.Result;
using Domain.Core.User;

namespace Domain.Core.Subscription;

public interface ISubscriptionRepository
{
    Task<Result<SubscriptionEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<SubscriptionEntity>> FindByUserAsync(UserEntity user, CancellationToken cancellationToken);

    void Insert(SubscriptionEntity subscription);

    void Update(SubscriptionEntity subscription);
}
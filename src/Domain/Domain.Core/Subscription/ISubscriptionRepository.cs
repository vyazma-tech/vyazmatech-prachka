using Domain.Common.Result;
using Domain.Core.User;

namespace Domain.Core.Subscription;

public interface ISubscriptionRepository
{
    Task<Result<SubscriptionEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<SubscriptionEntity>> FindByUserAsync(UserEntity user, CancellationToken cancellationToken);

    Task InsertAsync(SubscriptionEntity subscription, CancellationToken cancellationToken);

    void Update(SubscriptionEntity subscription);
}
using Domain.Common.Result;
using Domain.Core.Subscriber;
using Domain.Core.User;

namespace Application.DataAccess.Contracts.Repositories;

public interface ISubscriptionRepository
{
    Task<Result<SubscriberEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<SubscriberEntity>> FindByUserAsync(UserEntity user, CancellationToken cancellationToken);

    Task InsertAsync(SubscriberEntity subscriber, CancellationToken cancellationToken);

    Task Update(SubscriberEntity subscriber, CancellationToken cancellationToken);
}
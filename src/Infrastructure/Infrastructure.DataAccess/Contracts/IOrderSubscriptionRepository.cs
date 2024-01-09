using Domain.Common.Result;
using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IOrderSubscriptionRepository
{
    Task<Result<SubscriptionEntity>> FindByAsync(
        Specification<OrderSubscriptionModel> specification,
        CancellationToken cancellationToken);

    void Insert(SubscriptionEntity subscription);

    void Update(SubscriptionEntity subscription);
}
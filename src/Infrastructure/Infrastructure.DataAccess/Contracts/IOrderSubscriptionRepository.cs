using Domain.Common.Result;
using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IOrderSubscriptionRepository
{
    Task<Result<OrderSubscriptionEntity>> FindByAsync(
        Specification<OrderSubscriptionModel> specification,
        CancellationToken cancellationToken);

    void Insert(OrderSubscriptionEntity subscription);

    void Update(OrderSubscriptionEntity subscription);
}
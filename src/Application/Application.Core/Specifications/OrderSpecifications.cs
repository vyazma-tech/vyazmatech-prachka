using Application.DataAccess.Contracts.Querying.Order;
using Application.DataAccess.Contracts.Repositories;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;

namespace Application.Core.Specifications;

public static class OrderSpecifications
{
    public static async Task<Result<OrderEntity>> FindByIdAsync(
        this IOrderRepository repository,
        Guid id,
        CancellationToken token)
    {
        var query = OrderQuery.Build(x => x.WithId(id));

        OrderEntity? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
        {
            return new Result<OrderEntity>(DomainErrors.Entity.NotFoundFor<OrderEntity>(id.ToString()));
        }

        return result;
    }
}
using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Order;

namespace VyazmaTech.Prachka.Application.Core.Specifications;

public static class OrderSpecifications
{
    public static async Task<OrderEntity> FindByIdAsync(
        this IOrderRepository repository,
        Guid id,
        CancellationToken token)
    {
        var query = OrderQuery.Build(x => x.WithId(id));

        OrderEntity? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
            throw new NotFoundException(DomainErrors.Entity.NotFoundFor<OrderEntity>(id.ToString()));

        return result;
    }
}
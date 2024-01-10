using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Order;
using static Application.Handlers.Order.Queries.OrderById.OrderByIdQuery;

namespace Application.Handlers.Order.Queries.OrderById;

internal sealed class OrderByIdQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;

    public OrderByIdQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> result = await _persistenceContext.Orders
            .FindByAsync(
                new OrderByIdSpecification(request.Id),
                cancellationToken);

        if (result.IsFaulted)
            return new Result<Response>(result.Error);

        OrderEntity order = result.Value;

        return order.ToDto();
    }
}
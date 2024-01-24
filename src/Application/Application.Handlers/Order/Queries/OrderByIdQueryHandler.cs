using Application.Core.Contracts.Common;
using Application.Core.Specifications;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using static Application.Core.Contracts.Orders.Queries.OrderById;

namespace Application.Handlers.Order.Queries;

internal sealed class OrderByIdQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;

    public OrderByIdQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> result = await _persistenceContext.Orders.FindByIdAsync(request.Id, cancellationToken);

        if (result.IsFaulted)
            return new Result<Response>(result.Error);

        OrderEntity order = result.Value;

        return order.ToDto();
    }
}
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Queries.OrderById;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Queries;

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

        return new Response(order.ToDto());
    }
}
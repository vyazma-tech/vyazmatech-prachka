using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries.OrderById;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Queries;

internal sealed class OrderByIdQueryHandler : IQueryHandler<OrderById.Query, OrderById.Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public OrderByIdQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        Domain.Core.Orders.Order order = await _persistenceContext.Orders.GetByIdAsync(request.Id, cancellationToken);

        return new Response(order.ToDto());
    }
}
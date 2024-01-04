using Application.Core.Querying.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Queries.Links;

// TODO: FIX IT
// public class OrderPageQueryLink : QueryLinkBase<OrderQuery.QueryBuilder, OrderQueryParameter>
// {
//     private readonly ILogger<OrderPageQueryLink> _logger;
//
//     public OrderPageQueryLink(ILogger<OrderPageQueryLink> logger)
//     {
//         _logger = logger;
//     }
//
//     protected override OrderQuery.QueryBuilder TryApply(OrderQuery.QueryBuilder requestQueryable, OrderQueryParameter requestParameter)
//     {
//         throw new NotImplementedException();
//     }
// }
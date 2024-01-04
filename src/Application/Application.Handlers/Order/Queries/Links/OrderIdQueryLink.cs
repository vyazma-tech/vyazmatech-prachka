using Application.Core.Querying.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Queries.Links;

// TODO: FIX IT
// public class OrderIdQueryLink : QueryLinkBase<OrderQuery.QueryBuilder, OrderQueryParameter>
// {
//     private readonly ILogger<OrderIdQueryLink> _logger;
//
//     public OrderIdQueryLink(ILogger<OrderIdQueryLink> logger)
//     {
//         _logger = logger;
//     }
//
//     protected override OrderQuery.QueryBuilder TryApply(OrderQuery.QueryBuilder requestQueryable, OrderQueryParameter requestParameter)
//     {
//         throw new NotImplementedException();
//     }
// }
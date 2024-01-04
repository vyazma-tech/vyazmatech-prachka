using Application.Core.Querying.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Queries.Links;

// TODO: FIX IT
// public class CreationDateQueryLink : QueryLinkBase<OrderQuery.QueryBuilder, OrderQueryParameter>
// {
//     private readonly ILogger<CreationDateQueryLink> _logger;
//
//     public CreationDateQueryLink(ILogger<CreationDateQueryLink> logger)
//     {
//         _logger = logger;
//     }
//
//     protected override OrderQuery.QueryBuilder TryApply(OrderQuery.QueryBuilder requestQueryable, OrderQueryParameter requestParameter)
//     {
//         throw new NotImplementedException();
//     }
// }
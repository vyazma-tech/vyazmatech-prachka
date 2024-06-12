using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Queries.OrderByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.Order;

public class PageQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.Page is not null)
        {
            builder = builder.WithPage(query.Page.Value);
        }

        return builder;
    }
}
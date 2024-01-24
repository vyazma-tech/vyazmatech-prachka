using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Order;
using static Application.Core.Contracts.Orders.Queries.OrderByQuery;

namespace Application.Core.Querying.Order;

public class StatusQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.Status is not null)
            builder = builder.WithStatus(query.Status);

        return builder;
    }
}
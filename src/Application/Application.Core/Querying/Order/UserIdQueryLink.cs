using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Order;
using static Application.Core.Contracts.Orders.Queries.OrderByQuery;

namespace Application.Core.Querying.Order;

public class UserIdQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.UserId is not null)
            builder = builder.WithUserId(query.UserId.Value);

        return builder;
    }
}
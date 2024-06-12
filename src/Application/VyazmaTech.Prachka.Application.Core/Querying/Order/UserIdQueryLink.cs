using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Queries.OrderByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.Order;

public class UserIdQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.UserId is not null)
        {
            builder = builder.WithUserId(query.UserId.Value);
        }

        return builder;
    }
}
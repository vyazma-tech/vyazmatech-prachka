using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Queue;
using static Application.Core.Contracts.Queues.Queries.QueueByQuery;

namespace Application.Core.Querying.Queue;

public class OrderIdQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.OrderId is not null)
            builder = builder.WithOrderId(query.OrderId.Value);

        return builder;
    }
}
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.Queue;

public class OrderIdQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.OrderId is not null)
        {
            builder = builder.WithOrderId(query.OrderId.Value);
        }

        return builder;
    }
}
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.Queue;

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
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Queue;
using static Application.Core.Contracts.Queues.Queries.QueueByQuery;

namespace Application.Core.Querying.Queue;

public class PageQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.Page is not null)
            builder = builder.WithPage(query.Page.Value);

        return builder;
    }
}
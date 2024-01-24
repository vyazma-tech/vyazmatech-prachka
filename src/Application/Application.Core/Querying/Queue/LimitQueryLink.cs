using Application.Core.Configuration;
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Queue;
using Microsoft.Extensions.Options;
using static Application.Core.Contracts.Queues.Queries.QueueByQuery;

namespace Application.Core.Querying.Queue;

public class LimitQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    private readonly IOptions<PaginationConfiguration> _paginationConfiguration;

    public LimitQueryLink(IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _paginationConfiguration = paginationConfiguration;
    }

    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        int limit = _paginationConfiguration.Value.RecordsPerPage;
        builder = builder.WithLimit(limit);

        return builder;
    }
}
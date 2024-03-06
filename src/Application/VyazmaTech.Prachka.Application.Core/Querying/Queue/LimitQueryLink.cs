using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.Queue;

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
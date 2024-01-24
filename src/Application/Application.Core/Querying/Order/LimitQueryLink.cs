using Application.Core.Configuration;
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Order;
using Microsoft.Extensions.Options;
using static Application.Core.Contracts.Orders.Queries.OrderByQuery;

namespace Application.Core.Querying.Order;

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
using Application.Core.Configuration;
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.User;
using Microsoft.Extensions.Options;
using static Application.Core.Contracts.Users.Queries.UserByQuery;

namespace Application.Core.Querying.User;

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
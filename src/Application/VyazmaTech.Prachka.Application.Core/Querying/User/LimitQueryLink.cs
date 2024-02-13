using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.User;

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
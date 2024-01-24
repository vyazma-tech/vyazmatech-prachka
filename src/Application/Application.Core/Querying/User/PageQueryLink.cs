using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.User;
using static Application.Core.Contracts.Users.Queries.UserByQuery;

namespace Application.Core.Querying.User;

public class PageQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.Page is not null)
            builder = builder.WithPage(query.Page.Value);

        return builder;
    }
}
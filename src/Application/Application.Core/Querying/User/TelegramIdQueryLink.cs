using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.User;
using static Application.Core.Contracts.Users.Queries.UserByQuery;

namespace Application.Core.Querying.User;

public class TelegramIdQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.TelegramId is not null)
            builder = builder.WithTelegramId(query.TelegramId);

        return builder;
    }
}
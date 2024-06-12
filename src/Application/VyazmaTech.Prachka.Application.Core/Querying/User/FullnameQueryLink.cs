using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.User;

public class FullnameQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.Fullname is not null)
        {
            builder = builder.WithFullname(query.Fullname);
        }

        return builder;
    }
}
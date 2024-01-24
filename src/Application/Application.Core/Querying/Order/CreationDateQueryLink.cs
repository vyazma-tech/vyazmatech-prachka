using Application.Core.Querying.Common;
using Application.DataAccess.Contracts.Querying.Order;
using Domain.Common.Abstractions;
using static Application.Core.Contracts.Orders.Queries.OrderByQuery;

namespace Application.Core.Querying.Order;

public class CreationDateQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.CreationDate is not null)
            builder = builder.WithCreationDate(new SpbDateTime(query.CreationDate.Value));

        return builder;
    }
}
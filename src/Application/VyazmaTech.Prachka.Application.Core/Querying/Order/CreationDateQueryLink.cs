using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Queries.OrderByQuery;

namespace VyazmaTech.Prachka.Application.Core.Querying.Order;

public class CreationDateQueryLink : QueryLinkBase<Query, IQueryBuilder>
{
    protected override IQueryBuilder Apply(Query query, IQueryBuilder builder)
    {
        if (query.CreationDate is not null)
            builder = builder.WithCreationDate(new SpbDateTime(query.CreationDate.Value));

        return builder;
    }
}
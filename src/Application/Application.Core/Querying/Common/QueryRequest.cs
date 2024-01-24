namespace Application.Core.Querying.Common;

public record struct QueryRequest<TQuery, TQueryBuilder>(
    TQuery Query,
    TQueryBuilder Builder);

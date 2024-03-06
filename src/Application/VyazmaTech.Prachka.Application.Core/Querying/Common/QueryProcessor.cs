using FluentChaining;

namespace VyazmaTech.Prachka.Application.Core.Querying.Common;

public class QueryProcessor<TQuery, TQueryBuilder> : IQueryProcessor<TQuery, TQueryBuilder>
{
    private readonly IChain<QueryRequest<TQuery, TQueryBuilder>, TQueryBuilder> _chain;

    public QueryProcessor(IChain<QueryRequest<TQuery, TQueryBuilder>, TQueryBuilder> chain)
    {
        _chain = chain;
    }

    public TQueryBuilder Process(TQuery query, TQueryBuilder builder)
    {
        var request = new QueryRequest<TQuery, TQueryBuilder>(query, builder);
        builder = _chain.Process(request);

        return builder;
    }
}
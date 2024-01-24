using FluentChaining;

namespace Application.Core.Querying.Common;

public abstract class QueryLinkBase<TQuery, TQueryBuilder> : ILink<QueryRequest<TQuery, TQueryBuilder>, TQueryBuilder>
{
    public TQueryBuilder Process(
        QueryRequest<TQuery, TQueryBuilder> request,
        SynchronousContext context,
        LinkDelegate<QueryRequest<TQuery, TQueryBuilder>, SynchronousContext, TQueryBuilder> next)
    {
        TQueryBuilder result = Apply(request.Query, request.Builder);
        QueryRequest<TQuery, TQueryBuilder> newRequest = request with { Builder = result };

        return next.Invoke(newRequest, context);
    }

    protected abstract TQueryBuilder Apply(TQuery query, TQueryBuilder builder);
}
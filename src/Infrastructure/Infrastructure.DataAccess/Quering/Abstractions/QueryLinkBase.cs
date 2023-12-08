using FluentChaining;
using Infrastructure.DataAccess.Quering.Requests;

namespace Infrastructure.DataAccess.Quering.Abstractions;

public abstract class QueryLinkBase<TBuilder, TParameter>
    : ILink<ModelQueryRequest<TBuilder, TParameter>, TBuilder>
{
    public TBuilder Process(
        ModelQueryRequest<TBuilder, TParameter> request,
        SynchronousContext context,
        LinkDelegate<ModelQueryRequest<TBuilder, TParameter>, SynchronousContext, TBuilder> next)
    {
        TBuilder? result = TryApply(request.queryBuilder, request.parameter);
        return result ?? next.Invoke(request, context);
    }

    protected abstract TBuilder? TryApply(
        TBuilder requestQueryBuilder,
        QueryParameter<TParameter> requestParameter);
}
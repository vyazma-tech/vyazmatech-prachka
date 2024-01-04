using Application.Core.Querying.Requests;
using Domain.Kernel;
using FluentChaining;

namespace Application.Core.Querying.Abstractions;

public abstract class QueryLinkBase<TBuilder, TParameter>
    : ILink<EntityQueryRequest<TBuilder, TParameter>, TBuilder>
    where TBuilder : IQueryable<Entity>
{
    public TBuilder Process(
        EntityQueryRequest<TBuilder, TParameter> request,
        SynchronousContext context,
        LinkDelegate<EntityQueryRequest<TBuilder, TParameter>, SynchronousContext, TBuilder> next)
    {
        TBuilder? result = TryApply(request.QueryBuilder, request.Parameter);
        return result ?? next.Invoke(request, context);
    }

    protected abstract TBuilder? TryApply(
        TBuilder requestQueryable,
        TParameter requestParameter);
}
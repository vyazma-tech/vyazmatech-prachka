using Application.Core.Querying.Requests;
using FluentChaining;

namespace Application.Core.Querying.Abstractions;

public abstract class FilterLinkBase<TModel, TParameter> :
    ILink<EntityFilterRequest<TModel, TParameter>, IEnumerable<TModel>>
{
    public IEnumerable<TModel> Process(
        EntityFilterRequest<TModel, TParameter> request,
        SynchronousContext context,
        LinkDelegate<EntityFilterRequest<TModel, TParameter>, SynchronousContext, IEnumerable<TModel>> next)
    {
        IEnumerable<TModel>? result = TryApply(request.Data, request.Parameter);
        return result ?? next.Invoke(request, context);
    }

    protected abstract IEnumerable<TModel>? TryApply(
        IEnumerable<TModel> requestData,
        TParameter requestParameter);
}
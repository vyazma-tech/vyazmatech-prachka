using FluentChaining;
using Infrastructure.DataAccess.Quering.Requests;

namespace Infrastructure.DataAccess.Quering.Abstractions;

public abstract class FilterLinkBase<TModel, TParameter> :
    ILink<ModelFilterRequest<TModel, TParameter>, IEnumerable<TModel>>
{
    public IEnumerable<TModel> Process(
        ModelFilterRequest<TModel, TParameter> request,
        SynchronousContext context,
        LinkDelegate<ModelFilterRequest<TModel, TParameter>, SynchronousContext, IEnumerable<TModel>> next)
    {
        IEnumerable<TModel>? result = TryApply(request.data, request.parameter);
        return result ?? next.Invoke(request, context);
    }

    protected abstract IEnumerable<TModel>? TryApply(
        IEnumerable<TModel> requestData,
        QueryParameter<TParameter> requestParameter);
}
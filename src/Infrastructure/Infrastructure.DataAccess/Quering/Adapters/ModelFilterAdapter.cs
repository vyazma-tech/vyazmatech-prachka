using FluentChaining;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.DataAccess.Quering.Requests;

namespace Infrastructure.DataAccess.Quering.Adapters;

public class ModelFilterAdapter<TModel, TParameter> : IModelFilter<TModel, TParameter>
{
    private readonly IChain<ModelFilterRequest<TModel, TParameter>, IEnumerable<TModel>> _chain;

    public ModelFilterAdapter(IChain<ModelFilterRequest<TModel, TParameter>, IEnumerable<TModel>> chain)
    {
        _chain = chain;
    }

    public IEnumerable<TModel> Apply(IEnumerable<TModel> data, QueryConfiguration<TParameter> configuration)
    {
        foreach (QueryParameter<TParameter> parameter in configuration.parameters)
        {
            var request = new ModelFilterRequest<TModel, TParameter>(data, parameter);
            data = _chain.Process(request);
        }

        return data;
    }
}
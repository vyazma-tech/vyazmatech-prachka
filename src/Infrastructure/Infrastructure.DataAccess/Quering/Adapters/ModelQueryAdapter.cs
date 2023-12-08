using FluentChaining;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.DataAccess.Quering.Requests;

namespace Infrastructure.DataAccess.Quering.Adapters;

public class ModelQueryAdapter<TBuilder, TParameter> : IModelQuery<TBuilder, TParameter>
{
    private readonly IChain<ModelQueryRequest<TBuilder, TParameter>, TBuilder> _chain;

    public ModelQueryAdapter(IChain<ModelQueryRequest<TBuilder, TParameter>, TBuilder> chain)
    {
        _chain = chain;
    }

    public TBuilder Apply(TBuilder builder, QueryConfiguration<TParameter> configuration)
    {
        foreach (QueryParameter<TParameter> parameter in configuration.parameters)
        {
            var request = new ModelQueryRequest<TBuilder, TParameter>(builder, parameter);
            builder = _chain.Process(request);
        }

        return builder;
    }
}
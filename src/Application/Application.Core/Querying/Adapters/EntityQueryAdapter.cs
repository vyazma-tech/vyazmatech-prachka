using Application.Core.Querying.Abstractions;
using Application.Core.Querying.Requests;
using Domain.Kernel;
using FluentChaining;

namespace Application.Core.Querying.Adapters;

public class EntityQueryAdapter<TBuilder, TParameter> : IEntityQuery<TBuilder, TParameter>
{
    private readonly IChain<EntityQueryRequest<TBuilder, TParameter>, TBuilder> _chain;

    public EntityQueryAdapter(IChain<EntityQueryRequest<TBuilder, TParameter>, TBuilder> chain)
    {
        _chain = chain;
    }

    public TBuilder Apply(TBuilder builder, QueryConfiguration<TParameter> configuration)
    {
        var request = new EntityQueryRequest<TBuilder, TParameter>(builder, configuration.Parameter);
        builder = _chain.Process(request);

        return builder;
    }
}
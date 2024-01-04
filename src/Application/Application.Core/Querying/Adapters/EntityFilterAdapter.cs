using Application.Core.Querying.Abstractions;
using Application.Core.Querying.Requests;
using FluentChaining;

namespace Application.Core.Querying.Adapters;

public class EntityFilterAdapter<TModel, TParameter> : IEntityFilter<TModel, TParameter>
{
    private readonly IChain<EntityFilterRequest<TModel, TParameter>, IEnumerable<TModel>> _chain;

    public EntityFilterAdapter(IChain<EntityFilterRequest<TModel, TParameter>, IEnumerable<TModel>> chain)
    {
        _chain = chain;
    }

    public IEnumerable<TModel> Apply(IEnumerable<TModel> data, QueryConfiguration<TParameter> configuration)
    {
        var request = new EntityFilterRequest<TModel, TParameter>(data, configuration.Parameter);
        data = _chain.Process(request);

        return data;
    }
}
namespace Application.Core.Querying.Abstractions;

public interface IEntityFilter<TModel, TParameter>
{
    IEnumerable<TModel> Apply(
        IEnumerable<TModel> data,
        QueryConfiguration<TParameter> configuration);
}
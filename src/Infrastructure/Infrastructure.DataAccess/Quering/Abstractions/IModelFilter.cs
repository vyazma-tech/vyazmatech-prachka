namespace Infrastructure.DataAccess.Quering.Abstractions;

public interface IModelFilter<TModel, TParameter>
{
    IEnumerable<TModel> Apply(
        IEnumerable<TModel> data,
        QueryConfiguration<TParameter> configuration);
}
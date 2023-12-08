namespace Infrastructure.DataAccess.Quering.Abstractions;

public interface IModelQuery<TBuilder, TParameter>
{
    TBuilder Apply(TBuilder builder, QueryConfiguration<TParameter> configuration);
}
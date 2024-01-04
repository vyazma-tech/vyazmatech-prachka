using Domain.Kernel;

namespace Application.Core.Querying.Abstractions;

public interface IEntityQuery<TBuilder, TParameter>
{
    TBuilder Apply(TBuilder builder, QueryConfiguration<TParameter> configuration);
}
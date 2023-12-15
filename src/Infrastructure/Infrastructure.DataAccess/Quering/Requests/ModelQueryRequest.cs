using Infrastructure.DataAccess.Quering.Abstractions;

namespace Infrastructure.DataAccess.Quering.Requests;

public record struct ModelQueryRequest<TBuilder, TParameter>(
    TBuilder queryBuilder,
    QueryParameter<TParameter> parameter);
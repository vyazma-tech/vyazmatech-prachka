using Infrastructure.DataAccess.Quering.Abstractions;

namespace Infrastructure.DataAccess.Quering.Requests;

public record ModelFilterRequest<TModel, TParameter>(
    IEnumerable<TModel> data,
    QueryParameter<TParameter> parameter);
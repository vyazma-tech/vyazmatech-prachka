using Application.Core.Querying.Abstractions;

namespace Application.Core.Querying.Requests;

public record struct EntityQueryRequest<TBuilder, TParameter>(
    TBuilder QueryBuilder,
    TParameter Parameter);
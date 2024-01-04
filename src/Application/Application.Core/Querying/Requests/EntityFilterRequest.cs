using Application.Core.Querying.Abstractions;

namespace Application.Core.Querying.Requests;

public record EntityFilterRequest<TModel, TParameter>(
    IEnumerable<TModel> Data,
    TParameter Parameter);
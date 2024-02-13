using Mediator;

namespace VyazmaTech.Prachka.Application.Contracts.Common;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
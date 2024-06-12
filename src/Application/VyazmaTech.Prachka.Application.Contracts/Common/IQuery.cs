using Mediator;

namespace VyazmaTech.Prachka.Application.Contracts.Common;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
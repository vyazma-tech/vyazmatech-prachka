using Mediator;

namespace VyazmaTech.Prachka.Application.Contracts.Common;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
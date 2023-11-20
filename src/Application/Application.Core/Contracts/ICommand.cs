using Mediator;

namespace Application.Core.Contracts;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
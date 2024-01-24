using Mediator;

namespace Application.Core.Contracts.Common;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
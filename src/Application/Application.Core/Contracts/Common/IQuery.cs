using Mediator;

namespace Application.Core.Contracts.Common;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
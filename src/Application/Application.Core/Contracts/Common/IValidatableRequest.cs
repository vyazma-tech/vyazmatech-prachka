using Mediator;

namespace Application.Core.Contracts.Common;

public interface IValidatableRequest : IMessage
{
}

public interface IValidatableRequest<out TRequest> : IValidatableRequest, ICommand<TRequest>
{
}
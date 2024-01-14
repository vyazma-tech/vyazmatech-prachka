using Mediator;

namespace Application.Core.Contracts;

public interface IValidatableRequest : IMessage
{
}

public interface IValidatableRequest<out TRequest> : IValidatableRequest, ICommand<TRequest>
{
}
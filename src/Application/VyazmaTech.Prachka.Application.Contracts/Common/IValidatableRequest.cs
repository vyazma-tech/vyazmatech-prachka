using Mediator;

namespace VyazmaTech.Prachka.Application.Contracts.Common;

public interface IValidatableRequest : IMessage { }

public interface IValidatableRequest<out TRequest> : IValidatableRequest, ICommand<TRequest> { }
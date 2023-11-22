using Domain.Kernel;
using Mediator;

namespace Application.Core.Contracts;

public interface IEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}
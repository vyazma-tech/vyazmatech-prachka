using Domain.Kernel;
using Mediator;

namespace Application.Core.Contracts.Common;

public interface IEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}
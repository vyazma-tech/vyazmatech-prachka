using Mediator;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Application.Contracts.Common;

public interface IIntegrationEventHandler<in TIntegrationEvent> : INotificationHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
}
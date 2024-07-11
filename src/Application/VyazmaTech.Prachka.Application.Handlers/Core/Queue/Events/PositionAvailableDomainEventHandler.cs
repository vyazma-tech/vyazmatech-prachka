using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;

internal sealed class PositionAvailableDomainEventHandler : IEventHandler<PositionAvailableDomainEvent>
{
    public ValueTask Handle(PositionAvailableDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: publish notifications
        return ValueTask.CompletedTask;
    }
}
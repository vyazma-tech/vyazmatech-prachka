using Microsoft.AspNetCore.SignalR;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Presentation.Hubs.Queue;
using VyazmaTech.Prachka.Presentation.Hubs.Queue.Implementations;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;

internal sealed class QueueUpdatedDomainEventHandler : IEventHandler<QueueUpdatedDomainEvent>
{
    private readonly IHubContext<QueueHub, IQueueHubClient> _context;

    public QueueUpdatedDomainEventHandler(IHubContext<QueueHub, IQueueHubClient> context)
    {
        _context = context;
    }

    public async ValueTask Handle(QueueUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _context.Clients.All.ReceiveQueueUpdated(notification);
    }
}
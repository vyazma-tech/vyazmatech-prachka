using Microsoft.AspNetCore.SignalR;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Presentation.Hubs.Queue;
using VyazmaTech.Prachka.Presentation.Hubs.Queue.Implementations;
using VyazmaTech.Prachka.Presentation.Hubs.Queue.Models;

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
        var queueUpdatedModel = new QueueUpdatedModel(
            Id: notification.Id,
            AssignmentDate: notification.AssignmentDate,
            ActiveFrom: notification.ActivityBoundaries.ActiveFrom,
            ActiveUntil: notification.ActivityBoundaries.ActiveUntil,
            State: notification.State.ToString(),
            Capacity: notification.Capacity);

        await _context.Clients.All.ReceiveQueueUpdated(queueUpdatedModel);
    }
}
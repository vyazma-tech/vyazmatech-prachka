using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;

internal sealed class QueueExpiredDomainEventHandler : IEventHandler<QueueExpiredDomainEvent>
{
    private readonly IPersistenceContext _context;
    private readonly ILogger<QueueExpiredDomainEventHandler> _logger;

    public QueueExpiredDomainEventHandler(IPersistenceContext context, ILogger<QueueExpiredDomainEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async ValueTask Handle(QueueExpiredDomainEvent notification, CancellationToken cancellationToken)
    {
        Domain.Core.Queues.Queue queue = await _context.Queues.GetByIdAsync(notification.Id, cancellationToken);
        _logger.LogInformation("Queue with assignment date {@AssignmentDate} expired", queue.AssignmentDate);
        _logger.LogInformation("Going to remove unpaid orders");

        var unpaidOrders = queue.Orders
            .Where(x => x.Status == OrderStatus.New)
            .ToList();

        foreach (Domain.Core.Orders.Order? order in unpaidOrders)
        {
            order?.Dismiss();
        }

        if (queue.Capacity > queue.Orders.Count)
        {
            _logger.LogInformation("Queue has available positions, going to publish event");
            queue.Raise(new PositionAvailableDomainEvent(queue));
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Successfully removed {Quantity} orders from queue with assignment date {@AssignmentDate}",
            unpaidOrders.Count,
            queue.AssignmentDate);
    }
}
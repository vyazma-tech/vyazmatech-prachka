using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Events;

internal sealed class OrderReadyDomainEventHandler : IEventHandler<OrderReadyDomainEvent>
{
    private readonly IPersistenceContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderReadyDomainEventHandler> _logger;

    public OrderReadyDomainEventHandler(
        IPersistenceContext context,
        IUnitOfWork unitOfWork,
        ILogger<OrderReadyDomainEventHandler> logger)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async ValueTask Handle(OrderReadyDomainEvent notification, CancellationToken cancellationToken)
    {
        Domain.Core.Orders.Order order = await _context.Orders.GetByIdAsync(notification.Id, cancellationToken);

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        AssignmentDate assignmentDate = order.Queue.AssignmentDate;
        string userFullname = notification.Fullname;

        IQueryable<Domain.Core.Queues.Queue> queryable = _context.Entities<Domain.Core.Orders.Order>()
            .Include(x => x.Queue)
            .Include(x => x.User)
            .Select(x => x.Queue)
            .Where(x => x.AssignmentDate.Value > assignmentDate);

        IAsyncEnumerable<Domain.Core.Queues.Queue> allQueues = queryable.AsAsyncEnumerable();
        IReadOnlyCollection<Guid> ordersToDelete = await GetOrdersToDelete(allQueues, userFullname);

        try
        {
            await _context.Entities<Domain.Core.Orders.Order>()
                .Where(x => ordersToDelete.Contains(x.Id))
                .ExecuteDeleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogWarning("Concurrency exception occured when handling event {@Event}", notification);
            await transaction.RollbackAsync();
        }

        await transaction.CommitAsync(cancellationToken);
    }

    private async Task<IReadOnlyCollection<Guid>> GetOrdersToDelete(
        IAsyncEnumerable<Domain.Core.Queues.Queue> allQueues,
        string userFullname)
    {
        List<Guid> orders = [];
        await foreach (Domain.Core.Queues.Queue q in allQueues.Distinct())
        {
            IEnumerable<Guid> newOrders = q.Orders
                .Where(x =>
                    x.Status == OrderStatus.New &&
                    x.User.Fullname == userFullname)
                .Select(x => x.Id);

            orders.AddRange(newOrders);
        }

        return orders;
    }
}
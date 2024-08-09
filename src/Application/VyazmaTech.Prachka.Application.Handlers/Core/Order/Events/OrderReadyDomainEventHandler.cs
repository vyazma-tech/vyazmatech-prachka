using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Events;

internal sealed class OrderReadyDomainEventHandler : IEventHandler<OrderReadyDomainEvent>
{
    private readonly IPersistenceContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderReadyDomainEventHandler> _logger;

    public OrderReadyDomainEventHandler(
        IPersistenceContext context,
        IUnitOfWork unitOfWork,
        ILogger<OrderReadyDomainEventHandler> logger,
        DatabaseContext dbContext)
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

        List<Guid> ordersToDelete = await _context.Orders
            .QueryByUserAsync(order.User.Id, cancellationToken)
            .Where(x => x.Queue.AssignmentDate.Value > assignmentDate && x.User.Fullname == userFullname)
            .Select(x => x.Id)
            .ToListAsync();

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
}
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.User;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkRemoveOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkRemoveOrders;

internal sealed class BulkRemoveOrdersCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public BulkRemoveOrdersCommandHandler(IPersistenceContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        // TODO: в домен бы наверное это положить
        Guid? userId = _currentUser.Id;

        if (userId is null)
            throw new IdentityException(ApplicationErrors.BulkInsertOrders.AnonymousUserCantEnter);

        UserEntity user = await _context.Users.FindByIdAsync(userId.Value, cancellationToken);
        QueueEntity queue = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        var userOrders = queue.Orders.Where(x => x.User == user.Info).ToList();

        if (userOrders.Count < request.Quantity)
        {
            throw new DomainInvalidOperationException(
                ApplicationErrors.BulkRemoveOrders.UnableToRemoveWithExceededQuantity);
        }

        var query = OrderQuery.Build(x => x.WithUserId(userId.Value));

        List<OrderEntity> orders = await _context.Orders
            .QueryAsync(query, cancellationToken)
            .Take(request.Quantity)
            .ToListAsync(cancellationToken);

        foreach (OrderEntity order in orders)
        {
            queue.Remove(order);
        }

        _context.Orders.RemoveRange(orders);
        await _context.SaveChangesAsync(cancellationToken);

        return default;
    }
}
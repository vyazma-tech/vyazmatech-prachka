using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.User;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkRemoveOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkRemoveOrders;

internal sealed class BulkRemoveOrdersCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public BulkRemoveOrdersCommandHandler(IPersistenceContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Guid? userId = _currentUser.Id;

        if (userId is null)
        {
            return new Result<Response>(ApplicationErrors.BulkInsertOrders.AnonymousUserCantEnter);
        }

        Result<UserEntity> userSearchResult = await _context.Users.FindByIdAsync(userId.Value, cancellationToken);

        if (userSearchResult.IsFaulted)
        {
            return new Result<Response>(userSearchResult.Error);
        }

        Result<QueueEntity> queueSearchResult = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        if (queueSearchResult.IsFaulted)
        {
            return new Result<Response>(queueSearchResult.Error);
        }

        UserEntity user = userSearchResult.Value;
        QueueEntity queue = queueSearchResult.Value;

        var userOrders = queue.Orders.Where(x => x.User == user.Info).ToList();

        if (userOrders.Count < request.Quantity)
        {
            return new Result<Response>(ApplicationErrors.BulkRemoveOrders.UnableToRemoveWithExceededQuantity);
        }

        var query = OrderQuery.Build(x => x.WithUserId(userId.Value));

        List<OrderEntity> orders = await _context.Orders
            .QueryAsync(query, cancellationToken)
            .Take(request.Quantity)
            .ToListAsync(cancellationToken);

        foreach (OrderEntity order in orders)
        {
            Result<OrderEntity> result = queue.Remove(order);

            if (result.IsFaulted)
            {
                return new Result<Response>(result.Error);
            }
        }

        _context.Orders.RemoveRange(orders);
        await _context.SaveChangesAsync(cancellationToken);

        return default(Response);
    }
}
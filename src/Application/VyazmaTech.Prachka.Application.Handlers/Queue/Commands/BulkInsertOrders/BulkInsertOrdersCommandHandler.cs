using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.User;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkInsertOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkInsertOrders;

internal sealed class BulkInsertOrdersCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BulkInsertOrdersCommandHandler(
        IPersistenceContext context,
        ICurrentUser currentUser,
        IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Guid? userId = _currentUser.Id;

        if (userId is null)
            throw new IdentityException(ApplicationErrors.BulkInsertOrders.AnonymousUserCantEnter);

        UserEntity user = await _context.Users.FindByIdAsync(userId.Value, cancellationToken);
        QueueEntity queue = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        IReadOnlyCollection<OrderEntity> orders = CreateOrders(request, user, _dateTimeProvider);

        foreach (OrderEntity order in orders)
        {
            queue.Add(order, _dateTimeProvider.UtcNow);
        }

        _context.Orders.InsertRange(orders);
        await _context.SaveChangesAsync(cancellationToken);

        return default;
    }

    private static IReadOnlyCollection<OrderEntity> CreateOrders(
        Command request,
        UserEntity user,
        IDateTimeProvider dateTimeProvider)
    {
        var orders = new List<OrderEntity>();

        for (int i = 0; i < request.Quantity; i++)
        {
            var order = new OrderEntity(
                Guid.NewGuid(),
                request.QueueId,
                new UserInfo(
                    user.Id,
                    user.TelegramUsername,
                    user.Fullname),
                OrderStatus.New,
                dateTimeProvider.UtcNow);

            orders.Add(order);
        }

        return orders;
    }
}
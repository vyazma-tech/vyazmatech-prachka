using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
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

        Domain.Core.Users.User user = await _context.Users.GetByIdAsync(userId.Value, cancellationToken);
        Domain.Core.Queues.Queue queue = await _context.Queues.GetByIdAsync(request.QueueId, cancellationToken);

        IReadOnlyCollection<Domain.Core.Orders.Order> orders = CreateOrders(request, user, _dateTimeProvider);

        queue.BulkInsert(orders);

        _context.Orders.InsertRange(orders);
        await _context.SaveChangesAsync(cancellationToken);

        return default;
    }

    private static IReadOnlyCollection<Domain.Core.Orders.Order> CreateOrders(
        Command request,
        Domain.Core.Users.User user,
        IDateTimeProvider dateTimeProvider)
    {
        var orders = new List<Domain.Core.Orders.Order>();

        for (int i = 0; i < request.Quantity; i++)
        {
            var order = new Domain.Core.Orders.Order(
                Guid.NewGuid(),
                default!,
                user,
                OrderStatus.New,
                dateTimeProvider.UtcNow);

            orders.Add(order);
        }

        return orders;
    }
}
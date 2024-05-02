using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.User;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkInsertOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkInsertOrders;

internal sealed class BulkInsertOrdersCommandHandler : ICommandHandler<Command, Result<Response>>
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

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Guid? userId = _currentUser.Id;

        if (userId is null)
            return new Result<Response>(ApplicationErrors.BulkInsertOrders.AnonymousUserCantEnter);

        Result<UserEntity> userSearchResult = await _context.Users.FindByIdAsync(userId.Value, cancellationToken);

        if (userSearchResult.IsFaulted)
            return new Result<Response>(userSearchResult.Error);

        Result<QueueEntity> queueSearchResult = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        if (queueSearchResult.IsFaulted)
            return new Result<Response>(queueSearchResult.Error);

        UserEntity user = userSearchResult.Value;
        QueueEntity queue = queueSearchResult.Value;
        IReadOnlyCollection<OrderEntity> orders = CreateOrders(request, user, _dateTimeProvider);

        foreach (OrderEntity order in orders)
        {
            Result<OrderEntity> result = queue.Add(order, _dateTimeProvider.SpbDateTimeNow);

            if (result.IsFaulted)
                return new Result<Response>(result.Error);
        }

        _context.Orders.InsertRange(orders);
        await _context.SaveChangesAsync(cancellationToken);

        return default(Response);
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
                id: Guid.NewGuid(),
                queueId: request.QueueId,
                user: new UserInfo(
                    id: user.Id,
                    telegram: user.TelegramUsername,
                    fullname: user.Fullname),
                status: OrderStatus.New,
                creationDateTimeUtc: dateTimeProvider.SpbDateTimeNow);

            orders.Add(order);
        }

        return orders;
    }
}
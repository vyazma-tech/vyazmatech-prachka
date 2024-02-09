using Application.Core.Contracts.Common;
using Application.Core.Services;
using Application.Core.Specifications;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Kernel;
using static Application.Core.Contracts.Orders.Commands.ProlongOrder;

namespace Application.Handlers.Order.Commands.ProlongOrder;

internal sealed class ProlongOrderCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProlongOrderCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> orderSearchResult = await _persistenceContext.Orders
            .FindByIdAsync(request.OrderId, cancellationToken);

        if (orderSearchResult.IsFaulted)
            return new Result<Response>(orderSearchResult.Error);

        OrderEntity order = orderSearchResult.Value;

        Result<QueueEntity> prevQueueSearchResult = await _persistenceContext.Queues
            .FindByIdAsync(order.Queue, cancellationToken);

        if (prevQueueSearchResult.IsFaulted)
            return new Result<Response>(prevQueueSearchResult.Error);

        QueueEntity prevQueue = prevQueueSearchResult.Value;

        Result<QueueEntity> targetQueueSearchResult = await _persistenceContext.Queues
            .FindByIdAsync(request.TargetQueueId, cancellationToken);

        if (targetQueueSearchResult.IsFaulted)
            return new Result<Response>(targetQueueSearchResult.Error);

        QueueEntity targetQueue = targetQueueSearchResult.Value;

        var service = new OrderService(_persistenceContext.Orders, _dateTimeProvider);

        Result<OrderEntity> prolongOrderResult = service.ProlongOrder(order, prevQueue, targetQueue);

        if (prolongOrderResult.IsFaulted)
            return new Result<Response>(prolongOrderResult.Error);

        order = prolongOrderResult.Value;

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Services;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.ProlongOrder;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.ProlongOrder;

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

        return new Response(order.ToDto());
    }
}
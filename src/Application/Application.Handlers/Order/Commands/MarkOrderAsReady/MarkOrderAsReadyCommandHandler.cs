using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using Domain.Core.Order;
using Infrastructure.DataAccess.Specifications.Order;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<MarkOrderAsReadyCommand, Task>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<MarkOrderAsReadyCommandHandler> _logger;

    public MarkOrderAsReadyCommandHandler(
        IOrderRepository orderRepository,
        ILogger<MarkOrderAsReadyCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async ValueTask<Task> Handle(MarkOrderAsReadyCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderByIdResult = await _orderRepository
            .FindByAsync(idSpec, cancellationToken);
        
        if (orderByIdResult.IsFaulted)
        {
            _logger.LogWarning(
                "Order {Order} cannot be found due to: {Error}",
                orderByIdResult.Value,
                orderByIdResult.Error.Message);
        }

        Result<OrderEntity> makeReadyResult = orderByIdResult.Value.MakeReady(DateTime.UtcNow);
        if (makeReadyResult.IsFaulted)
        {
            _logger.LogWarning(
                "Order {Order} cannot be marked as ready due to: {Error}",
                makeReadyResult.Value,
                makeReadyResult.Error.Message);
        }
        
        return Task.CompletedTask;
    }
}
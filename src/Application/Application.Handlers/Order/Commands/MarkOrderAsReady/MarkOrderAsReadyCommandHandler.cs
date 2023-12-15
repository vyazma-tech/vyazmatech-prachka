using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Specifications.Order;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<MarkOrderAsReadyCommand, Task>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<MarkOrderAsReadyCommandHandler> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsReadyCommandHandler(
        IOrderRepository orderRepository,
        ILogger<MarkOrderAsReadyCommandHandler> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
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

        Result<OrderEntity> makeReadyResult = orderByIdResult.Value.MakeReady(_dateTimeProvider.UtcNow);
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
using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Specifications.Order;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<MarkOrderAsPaidCommand, Task>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<MarkOrderAsPaidCommandHandler> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsPaidCommandHandler(
        IOrderRepository orderRepository,
        ILogger<MarkOrderAsPaidCommandHandler> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Task> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
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

        Result<OrderEntity> makePaidResult = orderByIdResult.Value.MakePayment(_dateTimeProvider.UtcNow);
        if (makePaidResult.IsFaulted)
        {
            _logger.LogWarning(
                "Order {Order} cannot be marked as Paid due to: {Error}",
                makePaidResult.Value,
                makePaidResult.Error.Message);
        }

        return Task.CompletedTask;
    }
}
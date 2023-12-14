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
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsReadyCommandHandler(
        IOrderRepository orderRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _orderRepository = orderRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Task> Handle(MarkOrderAsReadyCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderByIdResult = await _orderRepository
            .FindByAsync(idSpec, cancellationToken);

        orderByIdResult.Value.MakeReady(_dateTimeProvider.UtcNow);

        return Task.CompletedTask;
    }
}
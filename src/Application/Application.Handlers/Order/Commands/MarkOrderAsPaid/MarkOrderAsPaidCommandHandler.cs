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
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsPaidCommandHandler(
        IOrderRepository orderRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _orderRepository = orderRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Task> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderByIdResult = await _orderRepository
            .FindByAsync(idSpec, cancellationToken);

        orderByIdResult.Value.MakePayment(_dateTimeProvider.UtcNow);

        return Task.CompletedTask;
    }
}
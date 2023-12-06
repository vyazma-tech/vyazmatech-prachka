using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using Domain.Core.Order;
using Infrastructure.DataAccess.Specifications.Order;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<MarkOrderAsReadyCommand, Result<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsReadyCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async ValueTask<Result<OrderResponse>> Handle(MarkOrderAsReadyCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderByIdResult = await _orderRepository
            .FindByAsync(idSpec, cancellationToken);
        if (orderByIdResult.IsSuccess)
        {
            Result<OrderEntity> makeReadyResult = orderByIdResult.Value.MakeReady(DateTime.UtcNow);
            if (makeReadyResult.IsSuccess)
                return new OrderResponse(makeReadyResult.Value.ToDto());
            return new Result<OrderResponse>(makeReadyResult.Error);
        }

        return new Result<OrderResponse>(orderByIdResult.Error);
    }
}
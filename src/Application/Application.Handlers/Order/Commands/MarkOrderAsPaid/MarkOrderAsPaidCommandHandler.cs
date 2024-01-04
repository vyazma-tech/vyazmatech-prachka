using Application.Core.Contracts;
using Application.DataAccess.Contracts;
using Application.Handlers.Mapping.OrderMapping;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Specifications.Order;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<MarkOrderAsPaidCommand, Result<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsPaidCommandHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = persistenceContext.Orders;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<OrderResponse>> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderByIdResult = await _orderRepository
            .FindByAsync(idSpec, cancellationToken);

        Result<OrderEntity> makePaidResult = orderByIdResult.Value.MakePayment(_dateTimeProvider.UtcNow);

        if (makePaidResult.IsSuccess)
        {
            _orderRepository.Update(makePaidResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new OrderResponse(makePaidResult.Value.ToDto());
        }
        else
        {
            return new Result<OrderResponse>(makePaidResult.Error);
        }
    }
}
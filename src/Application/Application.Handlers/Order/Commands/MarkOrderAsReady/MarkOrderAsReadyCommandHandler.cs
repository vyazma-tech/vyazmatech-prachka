using Application.Core.Contracts;
using Application.Handlers.Mapping.OrderMapping;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Order;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<MarkOrderAsReadyCommand, Result<OrderResponse>>
{
    private readonly IRepository<OrderEntity> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsReadyCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = _unitOfWork.GetRepository<OrderEntity>();
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<OrderResponse>> Handle(MarkOrderAsReadyCommand request, CancellationToken cancellationToken)
    {
        var idSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderByIdResult = await _orderRepository
            .FindByAsync(idSpec, cancellationToken);

        Result<OrderEntity> makeReadyResult = orderByIdResult.Value.MakeReady(_dateTimeProvider.UtcNow);

        if (makeReadyResult.IsSuccess)
        {
            _orderRepository.Update(makeReadyResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new OrderResponse(makeReadyResult.Value.ToDto());
        }
        else
        {
            return new Result<OrderResponse>(makeReadyResult.Error);
        }
    }
}
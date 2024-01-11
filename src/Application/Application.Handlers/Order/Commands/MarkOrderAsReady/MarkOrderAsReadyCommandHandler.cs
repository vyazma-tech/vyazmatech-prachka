using Application.Core.Contracts;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Order;
using static Application.Handlers.Order.Commands.MarkOrderAsReady.MarkOrderAsReady;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsReadyCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        IPersistenceContext persistenceContext)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = persistenceContext.Orders;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> searchResult = await _orderRepository
            .FindByAsync(
                new OrderByIdSpecification(request.Id),
                cancellationToken);

        if (searchResult.IsFaulted)
            return new Result<Response>(searchResult.Error);

        OrderEntity order = searchResult.Value;

        Result<OrderEntity> makeReadyResult = order.MakeReady(_dateTimeProvider.UtcNow);

        if (makeReadyResult.IsFaulted)
            return new Result<Response>(makeReadyResult.Error);

        order = makeReadyResult.Value;

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}
using Application.Core.Contracts;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Order;
using static Application.Handlers.Order.Commands.MarkOrderAsPaid.MarkOrderAsPaid;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<Command, Result<Response>>
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

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> searchResult = await _orderRepository
            .FindByAsync(
                new OrderByIdSpecification(request.Id),
                cancellationToken);

        if (searchResult.IsFaulted)
            return new Result<Response>(searchResult.Error);

        OrderEntity order = searchResult.Value;

        Result<OrderEntity> makePaidResult = order.MakePayment(_dateTimeProvider.UtcNow);

        if (makePaidResult.IsFaulted)
            return new Result<Response>(makePaidResult.Error);

        order = makePaidResult.Value;

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.MarkOrderAsPaid;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsPaidCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> searchResult = await _persistenceContext.Orders
            .FindByIdAsync(request.Id, cancellationToken);

        if (searchResult.IsFaulted)
        {
            return new Result<Response>(searchResult.Error);
        }

        OrderEntity order = searchResult.Value;

        Result<OrderEntity> makePaidResult = order.MakePayment(_dateTimeProvider.SpbDateTimeNow);

        if (makePaidResult.IsFaulted)
        {
            return new Result<Response>(makePaidResult.Error);
        }

        order = makePaidResult.Value;

        _persistenceContext.Orders.Update(order);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
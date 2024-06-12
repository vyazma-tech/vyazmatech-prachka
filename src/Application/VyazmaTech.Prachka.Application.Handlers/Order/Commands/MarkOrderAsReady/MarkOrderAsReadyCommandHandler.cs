using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.MarkOrderAsReady;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsReadyCommandHandler(
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

        Result<OrderEntity> makeReadyResult = order.MakeReady(_dateTimeProvider.UtcNow);

        if (makeReadyResult.IsFaulted)
        {
            return new Result<Response>(makeReadyResult.Error);
        }

        order = makeReadyResult.Value;

        _persistenceContext.Orders.Update(order);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands.ProlongOrder;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.ProlongOrder;

internal sealed class ProlongOrderCommandHandler : ICommandHandler<Command,
    Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public ProlongOrderCommandHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Orders.Order order = await _persistenceContext.Orders
            .GetByIdAsync(request.OrderId, cancellationToken);

        Domain.Core.Queues.Queue targetQueue = await _persistenceContext.Queues
            .GetByIdAsync(request.TargetQueueId, cancellationToken);

        order.ProlongInto(targetQueue);

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
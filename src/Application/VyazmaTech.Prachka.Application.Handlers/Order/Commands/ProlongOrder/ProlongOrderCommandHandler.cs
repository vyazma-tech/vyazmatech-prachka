using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.ProlongOrder;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.ProlongOrder;

internal sealed class ProlongOrderCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProlongOrderCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Orders.Order order = await _persistenceContext.Orders
            .FindByIdAsync(request.OrderId, cancellationToken);

        Domain.Core.Queues.Queue targetQueue = await _persistenceContext.Queues
            .FindByIdAsync(request.TargetQueueId, cancellationToken);

        order.Prolong(targetQueue, _dateTimeProvider.UtcNow);

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
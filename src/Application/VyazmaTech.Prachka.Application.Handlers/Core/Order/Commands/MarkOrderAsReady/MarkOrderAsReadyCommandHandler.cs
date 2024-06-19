using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands.MarkOrderAsReady;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.MarkOrderAsReady;

internal sealed class MarkOrderAsReadyCommandHandler : ICommandHandler<
    Contracts.Core.Orders.Commands.MarkOrderAsReady.Command, Contracts.Core.Orders.Commands.MarkOrderAsReady.Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public MarkOrderAsReadyCommandHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Orders.Order order = await _persistenceContext.Orders
            .GetByIdAsync(request.Id, cancellationToken);

        order.MakeReady();

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
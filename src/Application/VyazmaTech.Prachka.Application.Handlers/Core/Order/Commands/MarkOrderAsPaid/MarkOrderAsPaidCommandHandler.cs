using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands.MarkOrderAsPaid;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<
    Contracts.Core.Orders.Commands.MarkOrderAsPaid.Command, Contracts.Core.Orders.Commands.MarkOrderAsPaid.Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public MarkOrderAsPaidCommandHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Orders.Order order = await _persistenceContext.Orders
            .GetByIdAsync(request.Id, cancellationToken);

        order.MakePayment();

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.MarkOrderAsPaid;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<Command, Response>
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

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        OrderEntity order = await _persistenceContext.Orders
            .FindByIdAsync(request.Id, cancellationToken);

        order.MakePayment(_dateTimeProvider.UtcNow);

        _persistenceContext.Orders.Update(order);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
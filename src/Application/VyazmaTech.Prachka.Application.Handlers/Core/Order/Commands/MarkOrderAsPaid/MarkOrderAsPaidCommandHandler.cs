using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands.MarkOrderAsPaid;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<Command, Response>
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

        Domain.Core.Queues.Queue queue = order.Queue;
        int orderSerialNumber = queue.Orders.Where(x => x.User == order.User).ToList().IndexOf(order);
        string comment =
            $"ФИО: {order.User.Fullname.Value} TG: {order.User.TelegramUsername.Value} Порядковый номер: {orderSerialNumber + 1}";

        order.MakePayment(request.Price, comment);

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
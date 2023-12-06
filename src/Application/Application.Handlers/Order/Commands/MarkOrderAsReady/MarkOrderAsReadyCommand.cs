using Application.Core.Contracts;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

public sealed class MarkOrderAsReadyCommand : ICommand<Result<OrderResponse>>
{
    public MarkOrderAsReadyCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
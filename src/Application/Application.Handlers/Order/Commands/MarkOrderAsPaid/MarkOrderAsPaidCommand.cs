using Application.Core.Contracts;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

public sealed class MarkOrderAsPaidCommand : ICommand<Result<OrderResponse>>
{
    public MarkOrderAsPaidCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
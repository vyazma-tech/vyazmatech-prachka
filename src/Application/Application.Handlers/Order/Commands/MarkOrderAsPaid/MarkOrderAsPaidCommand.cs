using Application.Core.Contracts;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

public sealed class MarkOrderAsPaidCommand : ICommand<Task>
{
    public MarkOrderAsPaidCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
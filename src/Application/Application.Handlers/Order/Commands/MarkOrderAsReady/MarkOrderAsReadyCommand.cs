using Application.Core.Contracts;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

public sealed class MarkOrderAsReadyCommand : ICommand<Task>
{
    public MarkOrderAsReadyCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
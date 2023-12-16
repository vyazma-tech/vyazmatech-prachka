using Application.Core.Contracts;

namespace Application.Handlers.Order.Commands.CreateOrder;

public readonly record struct CreateOrderModel(
    Guid UserId,
    Guid QueueId,
    DateOnly CreationDate);

public sealed class CreateOrderCommand : ICommand<CreateOrderResponse>
{
    public CreateOrderCommand(IReadOnlyCollection<CreateOrderModel> orders)
    {
        Orders = orders;
    }

    public IReadOnlyCollection<CreateOrderModel> Orders { get; set; }
}
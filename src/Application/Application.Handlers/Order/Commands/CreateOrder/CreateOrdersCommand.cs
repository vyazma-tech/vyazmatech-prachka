using Application.Core.Contracts;

namespace Application.Handlers.Order.Commands.CreateOrder;

public readonly record struct CreateOrderModel(
    Guid UserId,
    Guid QueueId,
    DateOnly CreationDate);

public sealed class CreateOrdersCommand : ICommand<CreateOrdersResponse>
{
    public CreateOrdersCommand(IReadOnlyCollection<CreateOrderModel> orders)
    {
        Orders = orders;
    }

    public IReadOnlyCollection<CreateOrderModel> Orders { get; set; }
}
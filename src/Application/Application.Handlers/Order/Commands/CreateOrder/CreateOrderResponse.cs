
using Application.Handlers.Queue.Queries;

namespace Application.Handlers.Order.Commands.CreateOrder;

public readonly record struct CreateOrderResponseModel(
    Guid Id,
    bool Paid,
    bool Ready,
    DateTime? ModifiedOn,
    DateTime CreationDate);

public sealed record CreateOrderResponse(IReadOnlyCollection<CreateOrderResponseModel> OrderModel);

using Application.Handlers.Queue.Queries;

namespace Application.Handlers.Order.Commands.CreateOrder;

public readonly record struct CreateOrdersResponseModel(
    Guid Id,
    bool Paid,
    bool Ready,
    DateTime? ModifiedOn,
    DateOnly CreationDate);

public sealed record CreateOrdersResponse(IReadOnlyCollection<CreateOrdersResponseModel> OrderModel);
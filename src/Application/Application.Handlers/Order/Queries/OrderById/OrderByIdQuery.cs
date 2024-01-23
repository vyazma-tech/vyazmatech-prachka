using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;

namespace Application.Handlers.Order.Queries.OrderById;

public static class OrderByIdQuery
{
    public record Query(Guid Id) : IQuery<Result<Response>>;

    public record struct Response(
        Guid Id,
        Guid UserId,
        Guid QueueId,
        string Status,
        DateTime? ModifiedOn,
        DateOnly CreationDate);

    public static Response ToDto(this OrderEntity order)
    {
        return new Response(
            order.Id,
            order.User,
            order.Queue,
            order.Status.ToString(),
            order.ModifiedOn?.Value,
            order.CreationDate);
    }
}
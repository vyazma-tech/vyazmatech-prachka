using Application.Core.Contracts.Common;
using Domain.Common.Result;
using Domain.Core.Order;

namespace Application.Core.Contracts.Orders.Queries;

public static class OrderById
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
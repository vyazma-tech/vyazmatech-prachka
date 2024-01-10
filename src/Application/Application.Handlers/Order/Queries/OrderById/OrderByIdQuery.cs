using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;

namespace Application.Handlers.Order.Queries.OrderById;

public static class OrderByIdQuery
{
    public record struct Query(Guid Id) : IQuery<Result<Response>>;

    public record struct Response(
        Guid Id,
        Guid UserId,
        Guid QueueId,
        bool Paid,
        bool Ready,
        DateTime? ModifiedOn,
        DateOnly CreationDate);

    public static Response ToDto(this OrderEntity order)
    {
        return new Response(
            order.Id,
            order.User.Id,
            order.Queue.Id,
            order.Paid,
            order.Ready,
            order.ModifiedOn,
            order.CreationDate);
    }
}
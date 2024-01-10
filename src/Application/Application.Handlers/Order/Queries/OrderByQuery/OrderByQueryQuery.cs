using Application.Core.Common;
using Application.Core.Contracts;
using Domain.Core.Order;

namespace Application.Handlers.Order.Queries.OrderByQuery;

public static class OrderByQuery
{
    public record Query(Guid? UserId, Guid? QueueId, DateOnly? CreationDate, int Page) : IQuery<PagedResponse<Response>>;

    public record struct Response(
        Guid Id,
        bool Paid,
        bool Ready,
        DateTime? ModifiedOn,
        DateOnly CreationDate);

    public static Response ToDto(this OrderEntity order)
    {
        return new Response(
            order.Id,
            order.Paid,
            order.Ready,
            order.ModifiedOn,
            order.CreationDate);
    }
}
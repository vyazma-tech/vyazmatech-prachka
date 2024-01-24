using Application.Core.Common;
using Application.Core.Contracts.Common;
using Domain.Core.Order;

namespace Application.Core.Contracts.Orders.Queries;

public static class OrderByQuery
{
    public record Query(Guid? UserId, Guid? QueueId, DateTime? CreationDate, string? Status, int? Page) : IQuery<PagedResponse<Response>>;

    public record struct Response(
        Guid Id,
        Guid UserId,
        Guid QueueId,
        string Status,
        DateTime? ModifiedOn,
        DateTime CreationDate);

    public static Response ToDto(this OrderEntity order)
    {
        return new Response(
            order.Id,
            order.User,
            order.Queue,
            order.Status.ToString(),
            order.ModifiedOn?.Value,
            order.CreationDateTime.Value);
    }
}
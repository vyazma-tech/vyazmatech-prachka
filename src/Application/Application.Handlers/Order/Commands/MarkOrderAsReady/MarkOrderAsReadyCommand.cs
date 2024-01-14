using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

public static class MarkOrderAsReady
{
    public record Command(Guid Id) : IValidatableRequest<Result<Response>>;

    public record struct Response(
        Guid Id,
        Guid QueueId,
        string Status,
        DateOnly CreationDate,
        DateTime? ModifiedOn);

    public static Response ToDto(this OrderEntity order)
    {
        return new Response(
            order.Id,
            order.Queue.Id,
            order.Status.ToString(),
            order.CreationDate,
            order.ModifiedOn?.Value);
    }
}
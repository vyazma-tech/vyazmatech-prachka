using Application.Core.Contracts;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using Domain.Core.Order;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

public static class MarkOrderAsPaid
{
    public record struct Command(Guid Id) : ICommand<Result<Response>>;

    public record struct Response(
        Guid Id,
        Guid QueueId,
        bool Paid,
        bool Ready,
        DateOnly CreationDate,
        DateTime? ModifiedOn);

    public static Response ToDto(this OrderEntity order)
    {
        return new Response(
            order.Id,
            order.Queue.Id,
            order.Paid,
            order.Ready,
            order.CreationDate,
            order.ModifiedOn);
    }
}
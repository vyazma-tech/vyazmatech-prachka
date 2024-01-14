using Application.Core.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Result;
using Domain.Core.Queue;

namespace Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

public static class IncreaseQueueCapacity
{
    public record Command(Guid QueueId, int Capacity) : ICommand<Result<Response>>;

    public record struct Response(
        Guid Id,
        int Capacity,
        DateOnly AssignmentDate,
        DateTime? ModifiedOn,
        TimeOnly ActiveFrom,
        TimeOnly ActiveUntil);

    public static Response ToDto(this QueueEntity queue)
    {
        return new Response
        {
            Id = queue.Id,
            Capacity = queue.Capacity.Value,
            ModifiedOn = queue.ModifiedOn?.Value,
            AssignmentDate = queue.CreationDate,
            ActiveFrom = queue.ActivityBoundaries.ActiveFrom,
            ActiveUntil = queue.ActivityBoundaries.ActiveUntil,
        };
    }
}
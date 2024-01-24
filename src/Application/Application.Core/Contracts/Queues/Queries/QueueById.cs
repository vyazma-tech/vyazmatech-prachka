using Application.Core.Contracts.Common;
using Domain.Common.Result;
using Domain.Core.Queue;

namespace Application.Core.Contracts.Queues.Queries;

public static class QueueById
{
    public record Query(Guid Id) : IQuery<Result<Response>>;

    public record struct Response(
        Guid Id,
        int Capacity,
        long CurrentCapacity,
        string State,
        DateOnly AssignmentDate,
        DateTime? ModifiedOn,
        TimeOnly ActiveFrom,
        TimeOnly ActiveUntil);

    public static Response ToDto(this QueueEntity queue)
    {
        return new Response
        {
            Id = queue.Id,
            Capacity = queue.Capacity,
            CurrentCapacity = queue.Order.Count,
            State = queue.State.ToString(),
            ModifiedOn = queue.ModifiedOn?.Value,
            AssignmentDate = queue.CreationDate,
            ActiveFrom = queue.ActiveFrom,
            ActiveUntil = queue.ActiveUntil,
        };
    }
}
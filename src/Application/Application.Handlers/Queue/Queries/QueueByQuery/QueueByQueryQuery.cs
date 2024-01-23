using Application.Core.Common;
using Application.Core.Contracts;
using Domain.Core.Queue;

namespace Application.Handlers.Queue.Queries.QueueByQuery;

public static class QueueByQueryQuery
{
    public record Query(DateOnly? AssignmentDate, int Page) : IQuery<PagedResponse<Response>>;

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
            CurrentCapacity = queue.Orders.Count,
            ModifiedOn = queue.ModifiedOn?.Value,
            State = queue.State.ToString(),
            AssignmentDate = queue.CreationDate,
            ActiveFrom = queue.ActiveFrom,
            ActiveUntil = queue.ActiveUntil,
        };
    }
}
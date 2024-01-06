using Application.Core.Contracts;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using Domain.Core.Queue;

namespace Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;

public static class ChangeQueueActivityBoundaries
{
    public record struct Command(Guid QueueId, TimeOnly ActiveFrom, TimeOnly ActiveUntil) : ICommand<Response>;

    public record struct Response(
        Guid Id,
        int Capacity,
        bool Expired,
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
            Expired = queue.Expired,
            ModifiedOn = queue.ModifiedOn,
            AssignmentDate = queue.CreationDate,
            ActiveFrom = queue.ActivityBoundaries.ActiveFrom,
            ActiveUntil = queue.ActivityBoundaries.ActiveUntil,
        };
    }
}
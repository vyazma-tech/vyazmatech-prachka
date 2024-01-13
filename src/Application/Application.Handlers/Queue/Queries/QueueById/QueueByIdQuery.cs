﻿using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;

namespace Application.Handlers.Queue.Queries.QueueById;

public static class QueueByIdQuery
{
    public record Query(Guid Id) : IQuery<Result<Response>>;

    public record struct Response(
        Guid Id,
        int Capacity,
        long CurrentCapacity,
        DateOnly AssignmentDate,
        DateTime? ModifiedOn,
        TimeOnly ActiveFrom,
        TimeOnly ActiveUntil);

    public static Response ToDto(this QueueEntity queue, long currentCapacity)
    {
        return new Response
        {
            Id = queue.Id,
            Capacity = queue.Capacity.Value,
            CurrentCapacity = currentCapacity,
            ModifiedOn = queue.ModifiedOn,
            AssignmentDate = queue.CreationDate,
            ActiveFrom = queue.ActivityBoundaries.ActiveFrom,
            ActiveUntil = queue.ActivityBoundaries.ActiveUntil,
        };
    }
}
using Application.Handlers.Queue.Queries;
using Domain.Core.Queue;

namespace Application.Handlers.Mapping;

public static class QueueMapping
{
    public static QueueResponseModel ToDto(this QueueEntity queueEntity)
    {
        return new QueueResponseModel
        {
            Id = queueEntity.Id,
            Capacity = queueEntity.Capacity.Value,
            AssignmentDate = queueEntity.CreationDate,
            ActiveFrom = queueEntity.ActivityBoundaries.ActiveFrom,
            ActiveUntil = queueEntity.ActivityBoundaries.ActiveUntil,
        };
    }
}
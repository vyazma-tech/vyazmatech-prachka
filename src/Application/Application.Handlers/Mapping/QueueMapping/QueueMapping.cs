using Application.Handlers.Queue.Commands.CreateQueue;
using Application.Handlers.Queue.Queries;
using Domain.Core.Queue;

namespace Application.Handlers.Mapping.QueueMapping;

public static class QueueMapping
{
    public static QueueResponseModel ToDto(this QueueEntity queueEntity)
    {
        return new QueueResponseModel
        {
            Id = queueEntity.Id,
            Capacity = queueEntity.Capacity.Value,
            Expired = queueEntity.Expired,
            ModifiedOn = queueEntity.ModifiedOn,
            AssignmentDate = queueEntity.CreationDate,
            ActiveFrom = queueEntity.ActivityBoundaries.ActiveFrom,
            ActiveUntil = queueEntity.ActivityBoundaries.ActiveUntil,
        };
    }

    public static CreateQueueResponseModel ToCreationDto(this QueueEntity queueEntity)
    {
        return new CreateQueueResponseModel
        {
            Id = queueEntity.Id,
            Capacity = queueEntity.Capacity.Value,
            AssignmentDate = queueEntity.CreationDate,
            ActiveFrom = queueEntity.ActivityBoundaries.ActiveFrom,
            ActiveUntil = queueEntity.ActivityBoundaries.ActiveUntil,
        };
    }
}
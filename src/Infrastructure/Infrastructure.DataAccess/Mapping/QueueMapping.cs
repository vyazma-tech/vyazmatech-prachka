using Domain.Core.Queue;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class QueueMapping
{
    public static QueueEntity MapTo(QueueModel model, HashSet<Guid> orderIds)
    {
        return new QueueEntity(
            model.Id,
            model.Capacity,
            model.AssignmentDate,
            model.ActiveFrom,
            model.ActiveUntil,
            Enum.Parse<QueueState>(model.State),
            orderIds,
            model.MaxCapacityReached,
            model.ModifiedOn);
    }

    public static QueueModel MapFrom(QueueEntity entity)
    {
        return new QueueModel
        {
            Id = entity.Id,
            Capacity = entity.Capacity,
            AssignmentDate = entity.CreationDate,
            ActiveFrom = entity.ActiveFrom,
            ActiveUntil = entity.ActiveUntil,
            ModifiedOn = entity.ModifiedOn,
        };
    }
}
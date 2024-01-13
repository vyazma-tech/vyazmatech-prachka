using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Models;
using Infrastructure.Tools;

namespace Infrastructure.DataAccess.Mapping;

public static class QueueMapping
{
    public static QueueEntity MapTo(QueueModel model)
    {
        Capacity capacity = Capacity.Create(model.Capacity).Value;

        QueueDate assignmentDate = QueueDate.Create(
            model.AssignmentDate,
            new DateTimeProvider()).Value;

        QueueActivityBoundaries boundaries = QueueActivityBoundaries.Create(
            model.ActiveFrom,
            model.ActiveUntil).Value;

        return new QueueEntity(
            model.Id,
            capacity,
            assignmentDate,
            boundaries,
            model.ModifiedOn);
    }

    public static QueueModel MapFrom(QueueEntity entity)
    {
        return new QueueModel
        {
            Id = entity.Id,
            Capacity = entity.Capacity.Value,
            AssignmentDate = entity.CreationDate,
            ActiveFrom = entity.ActivityBoundaries.ActiveFrom,
            ActiveUntil = entity.ActivityBoundaries.ActiveUntil,
            ModifiedOn = entity.ModifiedOn,
            Orders = entity.Items.Select(OrderMapping.MapFrom).ToArray()
        };
    }
}
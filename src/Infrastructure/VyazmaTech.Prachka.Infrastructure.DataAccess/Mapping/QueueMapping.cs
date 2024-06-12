using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;

public static class QueueMapping
{
    public static Queue MapTo(QueueModel model, HashSet<OrderInfo> orderIds)
    {
        return new Queue(
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

    public static QueueModel MapFrom(Queue entity)
    {
        return new QueueModel
        {
            Id = entity.Id,
            Capacity = entity.Capacity,
            AssignmentDate = entity.CreationDate,
            ActiveFrom = entity.ActiveFrom,
            ActiveUntil = entity.ActiveUntil,
            ModifiedOn = entity.ModifiedOnUtc,
        };
    }
}
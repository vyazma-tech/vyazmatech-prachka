using Domain.Core.Order;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class OrderMapping
{
    public static OrderEntity MapTo(OrderModel model)
    {
        return OrderEntity.Create(
            model.Id,
            UserMapping.MapTo(model.User),
            QueueMapping.MapTo(model.Queue),
            model.CreationDate,
            model.ModifiedOn,
            model.Ready,
            model.Paid).Value;
    }

    public static OrderModel MapFrom(OrderEntity entity)
    {
        return new OrderModel
        {
            Id = entity.Id,
            QueueId = entity.Queue.Id,
            UserId = entity.User.Id,
            CreationDate = entity.CreationDateTime,
            ModifiedOn = entity.ModifiedOn,
            Paid = entity.Paid,
            Ready = entity.Ready
        };
    }
}
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.User;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;

public static class OrderMapping
{
    public static OrderEntity MapTo(OrderModel model)
    {
        return new OrderEntity(
            model.Id,
            model.QueueId,
            new UserInfo(model.UserId, model.User.TelegramUsername, model.User.Fullname),
            Enum.Parse<OrderStatus>(model.Status),
            model.CreationDate,
            model.ModifiedOn);
    }

    public static OrderModel MapFrom(OrderEntity entity)
    {
        return new OrderModel
        {
            Id = entity.Id,
            QueueId = entity.Queue,
            UserId = entity.User.Id,
            CreationDate = entity.CreationDateTime,
            ModifiedOn = entity.ModifiedOnUtc,
            Status = entity.Status.ToString(),
        };
    }
}
using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class OrderSubscriptionMapping
{
    public static OrderSubscriptionEntity MapTo(OrderSubscriptionModel model)
    {
        return new OrderSubscriptionEntity(
            model.Id,
            UserMapping.MapTo(model.User),
            model.CreationDate,
            model.ModifiedOn);
    }

    public static OrderSubscriptionModel MapFrom(OrderSubscriptionEntity entity)
    {
        return new OrderSubscriptionModel
        {
            Id = entity.Id,
            UserId = entity.User.Id,
            CreationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOn,
            Orders = entity.SubscribedOrders.Select(OrderMapping.MapFrom).ToArray()
        };
    }
}
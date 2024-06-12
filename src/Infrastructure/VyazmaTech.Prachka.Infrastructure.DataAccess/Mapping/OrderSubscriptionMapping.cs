using VyazmaTech.Prachka.Domain.Core.Subscriptions;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;

public static class OrderSubscriptionMapping
{
    public static OrderSubscriptionEntity MapTo(OrderSubscriptionModel model, HashSet<Guid> orderIds)
    {
        return new OrderSubscriptionEntity(
            model.Id,
            model.UserId,
            model.CreationDate,
            orderIds,
            model.ModifiedOn);
    }

    public static OrderSubscriptionModel MapFrom(OrderSubscriptionEntity entity)
    {
        return new OrderSubscriptionModel
        {
            Id = entity.Id,
            UserId = entity.User,
            CreationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOnUtc,
        };
    }
}
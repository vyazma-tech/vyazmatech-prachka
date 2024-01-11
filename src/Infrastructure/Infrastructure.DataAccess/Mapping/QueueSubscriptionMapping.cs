using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class QueueSubscriptionMapping
{
    public static QueueSubscriptionEntity MapTo(QueueSubscriptionModel model)
    {
        return new QueueSubscriptionEntity(
            model.Id,
            UserMapping.MapTo(model.User),
            model.CreationDate,
            model.ModifiedOn);
    }

    public static QueueSubscriptionModel MapFrom(QueueSubscriptionEntity entity)
    {
        return new QueueSubscriptionModel
        {
            Id = entity.Id,
            UserId = entity.User.Id,
            CreationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOn,
            Queues = entity.SubscribedQueues.Select(QueueMapping.MapFrom).ToArray()
        };
    }
}
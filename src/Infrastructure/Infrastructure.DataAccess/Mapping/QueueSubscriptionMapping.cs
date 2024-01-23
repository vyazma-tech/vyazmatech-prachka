using Domain.Core.Subscription;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class QueueSubscriptionMapping
{
    public static QueueSubscriptionEntity MapTo(QueueSubscriptionModel model, HashSet<Guid> queueIds)
    {
        return new QueueSubscriptionEntity(
            model.Id,
            model.UserId,
            model.CreationDate,
            queueIds,
            model.ModifiedOn);
    }

    public static QueueSubscriptionModel MapFrom(QueueSubscriptionEntity entity)
    {
        return new QueueSubscriptionModel
        {
            Id = entity.Id,
            UserId = entity.User,
            CreationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOn
        };
    }
}
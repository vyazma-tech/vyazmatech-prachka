using VyazmaTech.Prachka.Domain.Core.Subscription;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;

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
            ModifiedOn = entity.ModifiedOnUtc,
        };
    }
}
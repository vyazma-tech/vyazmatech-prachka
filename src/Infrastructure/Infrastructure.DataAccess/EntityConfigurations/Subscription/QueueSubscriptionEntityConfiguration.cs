using Domain.Core.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations.Subscription;

public sealed class QueueSubscriptionEntityConfiguration : IEntityTypeConfiguration<QueueSubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<QueueSubscriptionEntity> builder)
    {
        builder.HasMany(subscription => subscription.SubscribedQueues)
            .WithMany();
    }
}
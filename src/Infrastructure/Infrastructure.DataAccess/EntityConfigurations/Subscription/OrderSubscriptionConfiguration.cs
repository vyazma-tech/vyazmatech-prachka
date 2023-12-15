using Domain.Core.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations.Subscription;

public sealed class OrderSubscriptionConfiguration : IEntityTypeConfiguration<OrderSubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<OrderSubscriptionEntity> builder)
    {
        builder.HasMany(subscription => subscription.SubscribedOrders)
            .WithMany();
    }
}
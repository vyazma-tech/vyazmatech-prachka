using Domain.Core.Subscriber;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriberEntity>
{
    public void Configure(EntityTypeBuilder<SubscriberEntity> builder)
    {
        builder.HasOne(subscription => subscription.User);
        builder.HasOne(subscription => subscription.Queue);
        builder.HasMany(subscription => subscription.Orders)
            .WithOne();
        
        builder.Property(subscription => subscription.CreationDate);
        builder.Property(subscription => subscription.ModifiedOn);
    }
}
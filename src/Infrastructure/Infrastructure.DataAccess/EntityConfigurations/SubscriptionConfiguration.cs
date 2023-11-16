using Domain.Core.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
    {
        builder.HasOne(subscription => subscription.User)
            .WithOne()
            .HasForeignKey<SubscriptionEntity>()
            .IsRequired();

        builder.HasOne(subscription => subscription.Queue)
            .WithOne()
            .HasForeignKey<SubscriptionEntity>()
            .IsRequired(false);

        builder.HasMany(subscription => subscription.Orders)
            .WithOne();

        builder.Property(subscription => subscription.CreationDate);
        builder.Property(subscription => subscription.ModifiedOn);
    }
}
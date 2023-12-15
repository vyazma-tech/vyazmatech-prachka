using Domain.Core.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations.Subscription;

public class SubscriptionEntityConfiguration : IEntityTypeConfiguration<SubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
    {
        builder.HasOne(subscription => subscription.User)
            .WithOne()
            .HasForeignKey<SubscriptionEntity>()
            .IsRequired();

        builder.Property(subscription => subscription.CreationDate);
        builder.Property(subscription => subscription.ModifiedOn);
    }
}
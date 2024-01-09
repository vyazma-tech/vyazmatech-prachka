using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueSubscriptionConfiguration : IEntityTypeConfiguration<QueueSubscriptionModel>
{
    public void Configure(EntityTypeBuilder<QueueSubscriptionModel> builder)
    {
        builder.HasKey(subscription => subscription.Id);

        builder
            .HasOne(subscription => subscription.User)
            .WithOne(user => user.QueueSubscription)
            .HasForeignKey<QueueSubscriptionModel>(subscription => subscription.UserId)
            .IsRequired();

        builder
            .HasMany(subscription => subscription.Queues)
            .WithMany()
            .UsingEntity(join => join.ToTable("UserQueuesAndTheirSubscriptions"));
    }
}
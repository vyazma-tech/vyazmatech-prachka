using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

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
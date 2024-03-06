using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class OrderSubscriptionConfiguration : IEntityTypeConfiguration<OrderSubscriptionModel>
{
    public void Configure(EntityTypeBuilder<OrderSubscriptionModel> builder)
    {
        builder.HasKey(subscription => subscription.Id);

        builder
            .HasOne(subscription => subscription.User)
            .WithOne(user => user.OrderSubscription)
            .HasForeignKey<OrderSubscriptionModel>(subscription => subscription.UserId)
            .IsRequired();

        builder
            .HasMany(subscription => subscription.Orders)
            .WithMany()
            .UsingEntity(join => join.ToTable("UserOrdersAndTheirSubscriptions"));
    }
}
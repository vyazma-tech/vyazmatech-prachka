using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Domain.Core.Orders;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasOne(order => order.User)
            .WithMany()
            .HasForeignKey("user_id")
            .HasPrincipalKey(user => user.Id);

        builder
            .HasOne(order => order.Queue)
            .WithMany(queue => queue.Orders)
            .HasForeignKey("queue_id")
            .HasPrincipalKey(queue => queue.Id);

        builder.ComplexProperty(order => order.Price)
            .Property(x => x.Value)
            .HasColumnName("price");

        builder.Property(order => order.Status).HasDefaultValue(OrderStatus.New);
        builder.Property(order => order.CreationDateTime);
        builder.Property(order => order.CreationDate);
        builder.Property(order => order.ModifiedOnUtc);

        builder.HasIndex("queue_id", "user_id")
            .IsUnique(false);
    }
}
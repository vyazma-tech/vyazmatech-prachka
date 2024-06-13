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
            .HasForeignKey()
            .HasPrincipalKey(user => user.Id);

        builder
            .HasOne(order => order.Queue)
            .WithMany(queue => queue.Orders)
            .HasForeignKey()
            .HasPrincipalKey(queue => queue.Id);

        builder.Ignore(order => order.CreationDate);
        builder.Property(order => order.Status).HasDefaultValue(OrderStatus.New.ToString());
    }
}
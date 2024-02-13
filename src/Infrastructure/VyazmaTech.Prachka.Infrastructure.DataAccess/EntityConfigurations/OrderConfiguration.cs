using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<OrderModel>
{
    public void Configure(EntityTypeBuilder<OrderModel> builder)
    {
        builder.HasKey(order => order.Id);

        builder
            .HasOne(order => order.Queue)
            .WithMany(queue => queue.Orders)
            .HasForeignKey(order => order.QueueId)
            .HasPrincipalKey(queue => queue.Id);

        builder
            .HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId)
            .HasPrincipalKey(user => user.Id);

        builder.Property(order => order.Status).HasDefaultValue(OrderStatus.New.ToString());
    }
}
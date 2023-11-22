using Domain.Core.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasOne(order => order.User).WithMany();
        builder.Property(order => order.Paid);
        builder.Property(order => order.Ready);
        builder.HasIndex(order => order.CreationDate)
            .IsDescending();
        builder.Property(order => order.ModifiedOn);
    }
}
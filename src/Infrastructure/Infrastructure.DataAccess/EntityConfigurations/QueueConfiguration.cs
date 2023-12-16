using Domain.Core.Queue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueConfiguration : IEntityTypeConfiguration<QueueEntity>
{
    public void Configure(EntityTypeBuilder<QueueEntity> builder)
    {
        builder.HasMany(queue => queue.Items)
            .WithOne(order => order.Queue)
            .HasForeignKey("QueueId");

        builder.Navigation(queue => queue.Items).HasField("_orders");

        builder.Property(queue => queue.Capacity);

        builder.HasIndex(queue => queue.CreationDate)
            .IsDescending();

        builder.OwnsOne(queue => queue.ActivityBoundaries, navigationBuilder =>
        {
            navigationBuilder.Property(boundary => boundary.ActiveFrom)
                .HasColumnName("ActiveFrom");

            navigationBuilder.Property(boundary => boundary.ActiveUntil)
                .HasColumnName("ActiveUntil");
        })
            .Navigation(queue => queue.ActivityBoundaries).AutoInclude();

        builder.Property(queue => queue.ModifiedOn);
        builder.Ignore(queue => queue.Expired);
    }
}
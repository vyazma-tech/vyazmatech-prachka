using Domain.Core.Queue;
using Infrastructure.DataAccess.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueConfiguration : IEntityTypeConfiguration<QueueEntity>
{
    public void Configure(EntityTypeBuilder<QueueEntity> builder)
    {
        builder.HasMany(queue => queue.Items)
            .WithOne();

        builder.Property(queue => queue.Capacity);

        builder.HasIndex(queue => queue.CreationDate)
            .IsDescending();

        builder.OwnsOne(queue => queue.ActivityBoundaries, navigationBuilder =>
        {
            navigationBuilder.Property(boundary => boundary.ActiveFrom)
                .HasColumnName("ActiveFrom");

            navigationBuilder.Property(boundary => boundary.ActiveUntil)
                .HasColumnName("ActiveUntil");
        });

        builder.Property(queue => queue.ModifiedOn);
        builder.Property(queue => queue.Expired)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Domain.Core.Queues;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueConfiguration : IEntityTypeConfiguration<Queue>
{
    public void Configure(EntityTypeBuilder<Queue> builder)
    {
        builder.ComplexProperty(queue => queue.Capacity)
            .Property(x => x.Value)
            .HasColumnName("capacity");

        builder.ComplexProperty(
            queue => queue.ActivityBoundaries,
            propertyBuilder =>
            {
                propertyBuilder.Property(x => x.ActiveFrom).HasColumnName("active_from");
                propertyBuilder.Property(x => x.ActiveUntil).HasColumnName("active_until");
            });

        builder.ComplexProperty(queue => queue.AssignmentDate)
            .Property(x => x.Value)
            .HasColumnName("assignment_date");

        builder.Navigation(queue => queue.Orders).HasField("_orders");
        builder.Property(queue => queue.State).HasDefaultValue(QueueState.Prepared);
    }
}
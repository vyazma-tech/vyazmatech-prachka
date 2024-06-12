using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueConfiguration : IEntityTypeConfiguration<QueueModel>
{
    public void Configure(EntityTypeBuilder<QueueModel> builder)
    {
        builder.Property(queue => queue.State).HasDefaultValue(QueueState.Prepared);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueJobOutboxMessageConfiguration : IEntityTypeConfiguration<QueueJobMessage>
{
    public void Configure(EntityTypeBuilder<QueueJobMessage> builder)
    {
        builder.HasKey(message => message.Id);

        builder.HasIndex(
                message => new
                {
                    message.QueueId,
                    message.JobId
                })
            .IsUnique()
            .IsCreatedConcurrently();
    }
}
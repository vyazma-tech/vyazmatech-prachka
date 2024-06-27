using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Outbox;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueJobOutboxMessageConfiguration : IEntityTypeConfiguration<QueueJobOutboxMessage>
{
    public void Configure(EntityTypeBuilder<QueueJobOutboxMessage> builder)
    {
        builder.HasKey(message => message.QueueId);

        builder.HasIndex(message => message.ProcessedOnUtc)
            .HasFilter($"{nameof(QueueJobOutboxMessage.ProcessedOnUtc)} is not null".ToSnakeCase());
    }
}
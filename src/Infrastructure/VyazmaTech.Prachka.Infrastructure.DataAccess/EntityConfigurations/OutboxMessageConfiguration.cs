using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(message => message.Id);

        builder.HasQueryFilter(message => message.ProcessedOnUtc == null);

        builder.HasIndex(message => message.ProcessedOnUtc)
            .HasFilter($"{nameof(OutboxMessage.ProcessedOnUtc)} is null".ToSnakeCase());
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.Ignore(entity => entity.DomainEvents);
        builder.UseTpcMappingStrategy();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VyazmaTech.Prachka.Domain.Core.Users;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.EntityConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ComplexProperty(user => user.Fullname)
            .Property(x => x.Value)
            .HasColumnName("fullname");
    }
}
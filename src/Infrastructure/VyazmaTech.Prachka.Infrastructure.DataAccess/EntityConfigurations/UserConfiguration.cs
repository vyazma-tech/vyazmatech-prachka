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

        builder.ComplexProperty(user => user.TelegramUsername)
            .Property(x => x.Value)
            .HasColumnName("telegram_username");

        builder.Property(user => user.CreationDate);
        builder.Property(user => user.ModifiedOnUtc);

        builder.Property<string>("fullname");
        builder.Property<string>("telegram_username");
        builder.HasIndex("fullname", "telegram_username");
    }
}
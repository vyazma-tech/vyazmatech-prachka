using Domain.Core.User;
using Infrastructure.DataAccess.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(user => user.TelegramId);
        builder.Property(user => user.ModifiedOn);
        builder.Property(user => user.CreationDate);
    }
}
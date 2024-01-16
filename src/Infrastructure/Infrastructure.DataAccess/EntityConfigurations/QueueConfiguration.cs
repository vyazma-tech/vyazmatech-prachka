using Domain.Core.Queue;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityConfigurations;

public sealed class QueueConfiguration : IEntityTypeConfiguration<QueueModel>
{
    public void Configure(EntityTypeBuilder<QueueModel> builder)
    {
        builder.Property(queue => queue.State).HasDefaultValue(QueueState.Prepared);
    }
}
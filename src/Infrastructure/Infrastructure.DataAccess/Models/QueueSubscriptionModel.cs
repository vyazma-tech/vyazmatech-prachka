using Domain.Common.Abstractions;

#pragma warning disable CS8618

namespace Infrastructure.DataAccess.Models;

public record QueueSubscriptionModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public virtual UserModel User { get; set; }

    public DateOnly CreationDate { get; set; }

    public SpbDateTime? ModifiedOn { get; set; }

    public virtual ICollection<QueueModel> Queues { get; init; }
}
#pragma warning disable CS8618

namespace Infrastructure.DataAccess.Models;

public record OrderModel
{
    public Guid Id { get; init; }

    public Guid QueueId { get; init; }

    public virtual QueueModel Queue { get; set; }

    public Guid UserId { get; init; }

    public virtual UserModel User { get; set; }

    public bool Paid { get; set; }

    public bool Ready { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
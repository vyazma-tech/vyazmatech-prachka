using Application.DataAccess.Contracts;

#pragma warning disable CS8618

namespace Infrastructure.DataAccess.Models;

public record OrderModel
{
    public Guid Id { get; set; }

    public Guid QueueId { get; set; }

    public virtual QueueModel Queue { get; set; }

    public Guid UserId { get; set; }

    public virtual UserModel User { get; set; }

    public bool Paid { get; set; }

    public bool Ready { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
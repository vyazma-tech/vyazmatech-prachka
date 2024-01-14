#pragma warning disable CS8618

namespace Infrastructure.DataAccess.Models;

public record OrderModel
{
    public Guid Id { get; init; }

    public Guid QueueId { get; init; }

    public virtual QueueModel Queue { get; set; }

    public Guid UserId { get; init; }

    public virtual UserModel User { get; set; }

    // TODO: в одно поле стейта
    public string Status { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
using Domain.Common.Abstractions;
using Infrastructure.Tools;

#pragma warning disable CS8618

namespace Infrastructure.DataAccess.Models;

public record OrderModel
{
    public Guid Id { get; init; }

    public Guid QueueId { get; init; }

    public virtual QueueModel Queue { get; set; }

    public Guid UserId { get; init; }

    public virtual UserModel User { get; set; }

    public string Status { get; set; }

    public SpbDateTime CreationDate { get; set; }

    public SpbDateTime? ModifiedOn { get; set; }
}
#pragma warning disable CS8618

namespace Infrastructure.DataAccess.Models;

public record QueueModel
{
    public Guid Id { get; set; }

    public int Capacity { get; set; }

    public TimeOnly ActiveFrom { get; set; }

    public TimeOnly ActiveUntil { get; set; }

    public DateOnly AssignmentDate { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<OrderModel> Orders { get; init; }
}
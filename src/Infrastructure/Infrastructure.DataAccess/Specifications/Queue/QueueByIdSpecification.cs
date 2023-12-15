using Domain.Core.Queue;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByIdSpecification : Specification<QueueEntity>
{
    private readonly Guid _id;

    public QueueByIdSpecification(Guid id)
        : base(queue => queue.Id == id)
    {
        _id = id;
        AddInclude(queue => queue.ActivityBoundaries);
    }

    public override string ToString()
        => $"{typeof(QueueEntity)}: {_id}";
}
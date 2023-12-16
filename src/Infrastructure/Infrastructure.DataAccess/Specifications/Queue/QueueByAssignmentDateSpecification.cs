using Domain.Core.Queue;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByAssignmentDateSpecification : Specification<QueueEntity>
{
    public QueueByAssignmentDateSpecification(DateOnly assignmentDate)
        : base(queue => queue.CreationDate == assignmentDate)
    {
    }

    public override string ToString()
        => string.Empty;
}
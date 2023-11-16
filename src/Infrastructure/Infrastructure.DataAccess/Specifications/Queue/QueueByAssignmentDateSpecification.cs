using Domain.Core.Queue;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByAssignmentDateSpecification : Specification<QueueEntity>
{
    public QueueByAssignmentDateSpecification(DateTime assignmentDate) 
        : base(queue => queue.CreationDate == assignmentDate)
    {
    }

    public override string ToString()
        => string.Empty;
}
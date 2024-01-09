using Application.DataAccess.Contracts;
using Domain.Core.Queue;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByAssignmentDateSpecification : Specification<QueueModel>
{
    public QueueByAssignmentDateSpecification(DateOnly assignmentDate)
        : base(queue => queue.AssignmentDate == assignmentDate)
    {
    }

    public override string ToString()
        => string.Empty;
}
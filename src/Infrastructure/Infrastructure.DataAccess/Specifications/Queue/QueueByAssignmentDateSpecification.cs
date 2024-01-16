using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByAssignmentDateSpecification : Specification<QueueModel>
{
    private readonly DateOnly _assignmentDate;

    public QueueByAssignmentDateSpecification(DateOnly assignmentDate)
        : base(queue => queue.AssignmentDate == assignmentDate)
    {
        _assignmentDate = assignmentDate;
    }

    public override string ToString()
        => $"QueueAssignmentDate = {_assignmentDate}";
}
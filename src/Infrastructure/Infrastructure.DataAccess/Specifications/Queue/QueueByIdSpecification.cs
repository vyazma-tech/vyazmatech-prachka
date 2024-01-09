using Application.DataAccess.Contracts;
using Domain.Core.Queue;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByIdSpecification : Specification<QueueModel>
{
    private readonly Guid _id;

    public QueueByIdSpecification(Guid id)
        : base(queue => queue.Id == id)
    {
        _id = id;
    }

    public override string ToString()
        => $"{typeof(QueueModel)}: {_id}";
}
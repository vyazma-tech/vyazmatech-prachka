﻿using Domain.Common.Abstractions;
using Domain.Core.Queue;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByIdSpecification : Specification<QueueEntity>
{
    private readonly Guid _id;

    public QueueByIdSpecification(Guid id)
        : base(queue => queue.Id == id)
    {
        _id = id;
    }

    public override string ToString()
    {
        return $"{typeof(QueueEntity)}: {_id}";
    }
}
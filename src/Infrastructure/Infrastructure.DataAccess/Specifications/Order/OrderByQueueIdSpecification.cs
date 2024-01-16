using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByQueueIdSpecification : Specification<OrderModel>
{
    private readonly Guid _queueId;

    public OrderByQueueIdSpecification(Guid queueId)
        : base(x => x.Queue.Id == queueId)
    {
        _queueId = queueId;
        AddInclude(x => x.Queue);
    }

    public override string ToString()
        => $"QueueId = {_queueId}";
}